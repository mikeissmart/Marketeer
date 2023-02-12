namespace Marketeer.Core.Domain.Dtos.Market
{
    public class TickerFilterDto : IRefactorType
    {
        public string? Name { get; set; }
        public string? Symbol { get; set; }
        public string? QuoteType { get; set; }
        public string? Sector { get; set; }
        public string? Industry { get; set; }
        public bool? IsDelisted { get; set; }
    }
}
