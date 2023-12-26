using Marketeer.Core.Domain.Domain.CronJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketeer.Core.Domain.Dtos.Cron
{
    public class CronJobDetailDto : IRefactorType,
        IMapFrom<CronJobDetail>
    {
        public string Name { get; set; }
        public string CronExpression { get; set; }
        public DateTime? NextOccurrence { get; set; }
        public DateTime? LastOccurrence { get; set; }
        public bool IsRunning { get; set; }
    }
}
