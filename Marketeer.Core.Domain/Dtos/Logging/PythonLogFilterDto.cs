namespace Marketeer.Core.Domain.Dtos.Logging
{
    public class PythonLogFilterDto : IRefactorType
    {
        public string? File { get; set; }
        public bool? HasError { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
