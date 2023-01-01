using System.ComponentModel.DataAnnotations;

namespace Marketeer.Core.Domain.Entities.Logging
{
    public class CronLog : Entity
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(50)]
        public string? Message { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
