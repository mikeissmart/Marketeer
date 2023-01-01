using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Domain.Entities.Logging
{
    public class AppLog : Entity
    {
        public LogLevel LogLevel { get; set; }
        public int EventId { get; set; }
        public string? EventName { get; set; }
        public string? Source { get; set; }
        public string? StackTrace { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
