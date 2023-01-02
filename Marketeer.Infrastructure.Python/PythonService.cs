using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.Repositories.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Marketeer.Infrastructure.Python
{
    public interface IPythonService
    {
    }

    public abstract class BasePythonService : IPythonService
    {
        protected readonly IPythonLogRepository _pythonLogRepository;
        protected readonly PythonConfig _config;

        protected BasePythonService(PythonConfig config,
            IPythonLogRepository pythonLogRepository)
        {
            _pythonLogRepository = pythonLogRepository;
            _config = config;
        }

        protected async Task<TResut> RunPythonScriptAsync<TResut, TData>(string scriptFile, TData data)
        {
            var scriptFullPath = Path.Combine(_config.ScriptFolder, scriptFile);
            var log = new PythonLog
            {
                File = scriptFile,
                StartDate = DateTime.Now
            };

            var dataFile = await WriteDataFileAsync(data);
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = @"python.exe",
                    Arguments = $"\"{scriptFullPath}\" \"{dataFile}\"",
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                };

                using (var process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    await process.WaitForExitAsync();

                    using (var reader = process.StandardOutput)
                    {
                        log.Output = reader.ReadToEnd();
                    }
                    using (var reader = process.StandardError)
                    {
                        log.Error = reader.ReadToEnd();
                    }
                }

                return await ReadDataFileAsync<TResut>(dataFile);
            }
            catch (Exception e)
            {
                log.Error = e.Message;
                throw;
            }
            finally
            {
                File.Delete(dataFile);
                log.EndDate = DateTime.Now;
                await _pythonLogRepository.AddAsync(log);
                await _pythonLogRepository.SaveChangesAsync();
            }
        }

        private async Task<string> WriteDataFileAsync<T>(T data)
        {
            var dataFile = $"{Path.Combine(_config.DataFolder, Guid.NewGuid().ToString())}.json";
            await File.WriteAllTextAsync(dataFile, JsonConvert.SerializeObject(data));

            return dataFile;
        }

        private async Task<T> ReadDataFileAsync<T>(string dataFile)
        {
            var data = await File.ReadAllTextAsync(dataFile);
            File.Delete(dataFile);

            return JsonConvert.DeserializeObject<T>(data)!;
        }
    }
}
