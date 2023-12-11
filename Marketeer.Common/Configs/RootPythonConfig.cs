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
        private string _root = "";
        public string RootFolder
        {
            get
            {
                var a = Directory.GetCurrentDirectory();
                var b = string.Join("\\", a.Split("\\").Take(a.Split("\\").Count() - 1));
                var c = Path.Combine(b, "Marketeer.Infrastructure.Python\\Scripts");
                return c;
            }
            set => _root = value;
        }
        public string Venv { get; set; }
        public List<string> Packages { get; set; }

        public string RootVenvPath { get => Path.Combine(RootFolder, $"Venv\\{Venv}"); }
        public string PythonRootVenvPath { get => $"{Path.Combine(RootVenvPath, "Scripts\\python.exe")}"; }
    }
}
