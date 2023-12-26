using Marketeer.Core.Domain.Domain.CronJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Cron
{
    public class CronQueueDetailDto : IRefactorType,
        IMapFrom<CronQueueDetail>
    {
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public bool IsPendingStop { get; set; }
    }
}
