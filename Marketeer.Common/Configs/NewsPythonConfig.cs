using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Common.Configs
{
    public class NewsPythonConfig : PythonConfig, IConfig
    {
        public string FinvizFinanceNews { get; set; }
    }
}
