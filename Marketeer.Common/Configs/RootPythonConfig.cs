using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Common.Configs
{
    public class RootPythonConfig : IConfig
    {
        // TODO replace with proper way to get environment
        public string RootPath { get; set; }
        public string PythonRootPath { get => $"{Path.Combine(RootPath, "python.exe")}"; }
        public string ProjectPath
        {
            get
            {
                var a = Directory.GetCurrentDirectory();
                var b = string.Join("\\", a.Split("\\").Take(a.Split("\\").Count() - 1));
                var c = Path.Combine(b, "Marketeer.Infrastructure.Python");

                return c;
            }
        }
        public string ScriptsPath { get => Path.Combine(ProjectPath, "Scripts"); }
        public string VenvPath { get => Path.Combine(ScriptsPath, "Venv"); }
        public string PythonVenvPath { get => $"{Path.Combine(VenvPath, "Scripts\\python.exe")}"; }
        public List<string> Packages { get; set; }
    }
}
