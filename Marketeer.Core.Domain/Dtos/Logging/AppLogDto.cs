using Marketeer.Core.Domain.Entities.Logging;
using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Domain.Dtos.Logging
{
    public class AppLogDto : IRefactorType, IMapFrom<AppLog>
    {
        public LogLevel LogLevel { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? Source { get; set; }
        public string? StackTrace { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
