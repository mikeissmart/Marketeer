using Marketeer.Common.Configs;
using Marketeer.Persistance.Database.Repositories.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Infrastructure.Python
{
    public interface IPythonSetupService : IPythonService
    {
        Task CreatePythonEnvironmentAsync();
        Task InstallPackagesAsync();
    }

    public class PythonSetupService : BasePythonService, IPythonSetupService
    {
        public PythonSetupService(RootPythonConfig rootPythonConfig,
            IPythonLogRepository pythonLogRepository)
            : base(rootPythonConfig, pythonLogRepository)
        {
        }

        public async Task CreatePythonEnvironmentAsync()
        {
            Directory.CreateDirectory(_rootConfig.RootFolder);
            Directory.CreateDirectory(_rootConfig.RootVenvPath);

            var args = $" -m venv \"{_rootConfig.RootVenvPath}\"";
            await RunCommandAsync("python.exe", false, args, true);
            ;
        }

        public async Task InstallPackagesAsync()
        {
            foreach (var package in _rootConfig.Packages)
            {
                var args = $"pip install {package}";
                // will throw error if package updates are available
                await RunCommandAsync("python.exe", true, args, false);
            }
        }
    }
}
