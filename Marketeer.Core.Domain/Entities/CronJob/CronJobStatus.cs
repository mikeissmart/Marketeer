using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Entities.CronJob
{
    public class CronJobStatus : Entity
    {
        public string Name { get; set; }
        public bool IsRunning { get; set; }
    }
}
