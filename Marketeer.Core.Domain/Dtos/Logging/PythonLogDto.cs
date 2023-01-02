using Marketeer.Core.Domain.Entities.Logging;

namespace Marketeer.Core.Domain.Dtos.Logging
{
    public class PythonLogDto : IRefactorType, IMapFrom<PythonLog>, IMapTo<PythonLog>
    {
        public string File { get; set; }
        public string? Output { get; set; }
        public string? Error { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
