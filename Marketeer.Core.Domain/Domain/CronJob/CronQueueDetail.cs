using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Domain.CronJob
{
    public class CronQueueDetail
    {
        public Type CronQueueType { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public bool IsPendingStop { get; set; }
    }
}
