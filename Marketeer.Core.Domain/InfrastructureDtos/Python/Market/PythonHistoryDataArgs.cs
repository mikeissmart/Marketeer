namespace Marketeer.Core.Domain.InfrastructureDto.Python.Market
{
    public class PythonHistoryDataArgs
    {
        public string Ticker { get; set; }
        public string Interval { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
