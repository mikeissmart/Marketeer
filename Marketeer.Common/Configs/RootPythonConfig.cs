using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Common.Configs
{
    public class RootPythonConfig : IConfig
    {
        public string RootFolder { get; set; }
        public string Venv { get; set; }
        public List<string> Packages { get; set; }

        public string RootVenvPath { get => Path.Combine(RootFolder, $"Venv\\{Venv}"); }
        public string PythonRootVenvPath { get => $"{Path.Combine(RootVenvPath, "Scripts\\python.exe")}"; }
    }
}
