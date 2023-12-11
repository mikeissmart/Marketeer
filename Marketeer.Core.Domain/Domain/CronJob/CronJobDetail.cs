using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Domain.CronJob
{
    public class CronJobDetail
    {
        public Type CronJobType { get; set; }
        public string Name { get; set; }
        public string CronExpression { get; set; }
        public DateTime? NextOccurrence { get; set; }
        public DateTime? LastOccurrence { get; set; }
        public bool IsRunning { get; set; }
    }
}
