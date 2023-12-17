using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Common.Configs
{
    public class NewsPythonConfig : PythonConfig, IConfig
    {
        public string FinvizFinanceNewsLink { get; set; }
        public string FinvizFinanceNewsText { get; set; }
    }
}
