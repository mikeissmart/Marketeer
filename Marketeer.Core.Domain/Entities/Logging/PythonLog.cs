namespace Marketeer.Core.Domain.Entities.Logging
{
    public class PythonLog : Entity
    {
        public string File { get; set; }
        public string? Output { get; set; }
        public string? Error { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
