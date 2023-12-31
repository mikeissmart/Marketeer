using Marketeer.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Marketeer.Core.Domain.Entities.Logging
{
    public class CronLog : Entity
    {
        [StringLength(50)]
        public string Name { get; set; }
        public CronLogTypeEnum CronLogType { get; set; }
        public string? Message { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
