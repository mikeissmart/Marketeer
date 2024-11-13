using Marketeer.Common.Configs;
using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Persistance.Database.Repositories.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Marketeer.Infrastructure.Python
{
    public interface IPythonService
    {
        string GetDataFolder();
    }

    public abstract class BasePythonService : IPythonService
    {
        protected readonly IPythonLogRepository _pythonLogRepository;
        protected readonly RootPythonConfig _rootConfig;
        protected readonly PythonConfig _config;

        protected BasePythonService(RootPythonConfig rootPythonConfig,
            IPythonLogRepository pythonLogRepository)
        {
            _pythonLogRepository = pythonLogRepository;
            _rootConfig = rootPythonConfig;
        }

        protected BasePythonService(RootPythonConfig rootPythonConfig,
            PythonConfig config,
            IPythonLogRepository pythonLogRepository)
        {
            _pythonLogRepository = pythonLogRepository;
            _rootConfig = rootPythonConfig;
            _config = config;
        }

        public string GetDataFolder() => Path.Combine(_rootConfig.ScriptsPath, _config.DataFolder);

        protected async Task RunPythonScriptAsync(string scriptFile)
        {
            await RunPythonScriptAsync(
                scriptFile,
                null);
        }

        protected async Task<TResut> RunPythonScriptResultAsync<TResut>(string scriptFile)
        {
            var file = CreateArgsFilePath();

            await RunPythonScriptAsync(
                scriptFile,
                file);

            return ReadResultFile<TResut>(file);
        }

        protected async Task RunPythonScriptArgsAsync<TArgs>(string scriptFile, TArgs args)
        {
            var file = WriteArgsFile(args);

            await RunPythonScriptAsync(
                scriptFile,
                file);

            DeleteResultFile(file);
        }

        protected async Task<TResut> RunPythonScriptAsync<TResut, TArgs>(string scriptFile, TArgs args)
        {
            var file = WriteArgsFile(args);

            await RunPythonScriptAsync(
                scriptFile,
                file);

            return ReadResultFile<TResut>(file);
        }

        protected async Task RunCommandAsync(string command, bool useVenv, string args, bool throwErrors)
        {
            var log = new PythonLog
            {
                File = $"Command: {command}, Args: {args}",
                StartDateTime = DateTime.Now
            };

            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = useVenv
                        ? Path.Combine(_rootConfig.VenvPath, "Scripts\\" + command)
                        : command,
                    Arguments = args,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                };

                using (var process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();
                    log.Output = process.StandardOutput.ReadToEnd();
                    log.Error = process.StandardError.ReadToEnd();
                    await process.WaitForExitAsync();
                }

                if (throwErrors && !string.IsNullOrEmpty(log.Error))
                    throw new Exception(log.Error);
            }
            catch (Exception ex)
            {
                log.Error = ex.Message;
                throw;
            }
            finally
            {
                log.EndDateTime = DateTime.Now;
                await _pythonLogRepository.AddAsync(log);
                await _pythonLogRepository.SaveChangesAsync();
            }
        }

        private async Task RunPythonScriptAsync(string scriptFile, string? argsFile)
        {
            var log = new PythonLog
            {
                File = scriptFile,
                StartDateTime = DateTime.Now
            };
            var scriptFullPath = Path.Combine(_rootConfig.ScriptsPath, Path.Combine(_config.ScriptFolder, scriptFile));
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = _rootConfig.PythonVenvPath,
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
                    log.Output = process.StandardOutput.ReadToEnd();
                    log.Error = process.StandardError.ReadToEnd();
                    await process.WaitForExitAsync();
                }

                // Commenting for now
                // Lots of false positive errors
                // Even warnings produce error output
                //if (!string.IsNullOrEmpty(log.Error))
                //    throw new Exception(log.Error);
            }
            catch (Exception ex)
            {
                if (argsFile != null)
                    File.Delete(argsFile);

                log.Error = ex.Message;
                throw;
            }
            finally
            {
                log.EndDateTime = DateTime.Now;
                await _pythonLogRepository.AddAsync(log);
                await _pythonLogRepository.SaveChangesAsync();
            }
        }

        private string CreateArgsFilePath()
        {
            var path = GetDataFolder();
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var file = $"{Path.Combine(path, Guid.NewGuid().ToString())}.json";
            File.Create(file).Dispose();

            return file;
        }

        private string WriteArgsFile<TArgs>(TArgs args)
        {
            var dataFile = CreateArgsFilePath();
            File.WriteAllText(dataFile, JsonConvert.SerializeObject(args));

            return dataFile;
        }

        private TResult ReadResultFile<TResult>(string resultFile)
        {
            var data = File.ReadAllText(resultFile);
            File.Delete(resultFile);

            return JsonConvert.DeserializeObject<TResult>(data)!;
        }

        private void DeleteResultFile(string resultFile)
        {
            File.Delete(resultFile);
        }
    }
}
