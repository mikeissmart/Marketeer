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

        protected async Task RunPythonScriptAsync(string scriptFile)
        {
            await RunPythonScriptAsync(
                scriptFile,
                Path.Combine(_config.ScriptFolder, scriptFile),
                null);
        }

        protected async Task<TResut> RunPythonScriptResultAsync<TResut>(string scriptFile)
        {
            var file = GetArgsFilePath();

            await RunPythonScriptAsync(
                scriptFile,
                Path.Combine(_config.ScriptFolder, scriptFile),
                file);

            return await ReadResultFileAsync<TResut>(file);
        }

        protected async Task RunPythonScriptArgsAsync<TArgs>(string scriptFile, TArgs args)
        {
            var file = await WriteArgsFileAsync(args);

            await RunPythonScriptAsync(
                scriptFile,
                Path.Combine(_config.ScriptFolder, scriptFile),
                file);

            DeleteResultFile(file);
        }

        protected async Task<TResut> RunPythonScriptAsync<TResut, TArgs>(string scriptFile, TArgs args)
        {
            var file = await WriteArgsFileAsync(args);

            await RunPythonScriptAsync(
                scriptFile,
                Path.Combine(_config.ScriptFolder, scriptFile),
                file);

            return await ReadResultFileAsync<TResut>(file);
        }

        private async Task RunPythonScriptAsync(string scriptFile, string scriptFullPath, string? argsFile)
        {
            var log = new PythonLog
            {
                File = scriptFile,
                StartDate = DateTime.Now
            };
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = @"python.exe",
                    Arguments = argsFile != null
                        ? $"\"{scriptFullPath}\" \"{argsFile}\""
                        : $"\"{scriptFullPath}\"",
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
            }
            catch (Exception e)
            {
                log.Error = e.Message;
                throw;
            }
            finally
            {
                if (argsFile != null)
                    File.Delete(argsFile);

                log.EndDate = DateTime.Now;
                await _pythonLogRepository.AddAsync(log);
                await _pythonLogRepository.SaveChangesAsync();
            }
        }

        private string GetArgsFilePath()
        {
            return $"{Path.Combine(_config.DataFolder, Guid.NewGuid().ToString())}.json";
        }

        private async Task<string> WriteArgsFileAsync<TArgs>(TArgs args)
        {
            var dataFile = GetArgsFilePath();
            await File.WriteAllTextAsync(dataFile, JsonConvert.SerializeObject(args));

            return dataFile;
        }

        private async Task<TResult> ReadResultFileAsync<TResult>(string resultFile)
        {
            var data = await File.ReadAllTextAsync(resultFile);
            File.Delete(resultFile);

            return JsonConvert.DeserializeObject<TResult>(data)!;
        }

        private void DeleteResultFile(string resultFile)
        {
            File.Delete(resultFile);
        }
    }
}
