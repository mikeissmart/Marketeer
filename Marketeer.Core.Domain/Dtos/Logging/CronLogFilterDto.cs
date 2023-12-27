using Marketeer.Core.Domain.Enums;

namespace Marketeer.Core.Domain.Dtos.Logging
{
    public class CronLogFilterDto : IRefactorType
    {
        public string? Name { get; set; }
        public CronLogTypeEnum? CronLogType { get; set; }
        public bool? IsCanceled { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
