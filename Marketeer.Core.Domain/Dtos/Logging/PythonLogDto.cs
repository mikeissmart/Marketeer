using Marketeer.Core.Domain.Entities.Logging;

namespace Marketeer.Core.Domain.Dtos.Logging
{
    public class PythonLogDto : IMapFrom<PythonLog>
    {
        public string File { get; set; }
        public string ErrorOutput { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
