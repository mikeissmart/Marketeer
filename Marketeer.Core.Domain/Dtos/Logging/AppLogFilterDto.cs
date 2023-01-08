using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Domain.Dtos.Logging
{
    public class AppLogFilterDto : IRefactorType
    {
        public LogLevel? LogLevel { get; set; }
        public int? EventId { get; set; }
        public string? EventName { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
