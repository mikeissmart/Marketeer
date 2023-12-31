namespace Marketeer.Core.Domain.Entities.Logging
{
    public class PythonLog : Entity
    {
        public string File { get; set; }
        public string? Output { get; set; }
        public string? Error { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
