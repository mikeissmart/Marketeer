namespace Marketeer.Core.Domain.Entities.Logging
{
    public class PythonLog : Entity
    {
        public string File { get; set; }
        public string ErrorOutput { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
