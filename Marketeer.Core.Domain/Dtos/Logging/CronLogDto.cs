using Marketeer.Core.Domain.Entities.Logging;
using Marketeer.Core.Domain.Enums;

namespace Marketeer.Core.Domain.Dtos.Logging
{
    public class CronLogDto : IRefactorType, IMapFrom<CronLog>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CronLogTypeEnum CronLogType { get; set; }
        public string? Message { get; set; }
        public bool IsCanceled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
