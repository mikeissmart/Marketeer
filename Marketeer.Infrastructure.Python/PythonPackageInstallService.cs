using Marketeer.Common.Configs;
using Marketeer.Persistance.Database.Repositories.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            Directory.CreateDirectory(_rootConfig.RootVenvPath);

            var args = $" -m venv \"{_rootConfig.RootVenvPath}\"";
            await RunCommandAsync("python.exe", false, args, false);
        }

        public async Task InstallPackagesAsync()
        {
            var pipPath = Path.Combine(_rootConfig.RootVenvPath, "Scripts\\pip.exe");
            foreach (var package in _rootConfig.Packages)
            {
                var args = $" {pipPath} install \"{package}\"";
                await RunCommandAsync("python.exe", true, args, false);
            }
        }
    }
}
