namespace Marketeer.Core.Domain.Dtos
{
    public class PaginateDto<T> where T : class, IRefactorType
    {
        public int PageIndex { get; set; }
        public int PageItemCount { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemCount { get; set; }
        public List<T> Items { get; set; }
    }
}
