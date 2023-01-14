namespace Marketeer.Core.Domain.InfrastructureDtos.Python.Market
{
    public class PythonTickerInfoDto
    {
        public int TickerId { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string QuoteType { get; set; }
        public string Sector { get; set; }
        public string Industry { get; set; }
        public long Volume { get; set; }
    }
}
