using Marketeer.Core.Domain.Entities.Logging;
using Microsoft.Extensions.Logging;

namespace Marketeer.Core.Service.Loggers
{
    public class DbLogger : ILogger
    {
        private readonly DbLoggerProcessor _processor;

        public DbLogger(DbLoggerProcessor processor) => _processor = processor;

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= LogLevel.Warning;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var log = new AppLog
            {
                LogLevel = logLevel,
                EventId = eventId.Id,
                EventName = eventId.Name,
                CreatedDateTime = DateTime.Now
            };

            if (exception != null)
            {
                log.Source = exception.Source;
                log.StackTrace = exception.StackTrace;
            }
            log.Message = formatter.Invoke(state, exception);

            _processor.Enqueue(log);
        }
    }
}
