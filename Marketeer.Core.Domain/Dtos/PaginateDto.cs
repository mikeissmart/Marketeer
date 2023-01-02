namespace Marketeer.Core.Domain.Dtos
{
    // Map is defined in Common.Mapper.MappingProfile
    public class PaginateDto<T> : IRefactorType where T : class
    {
        public int PageIndex { get; set; }
        public int PageItemCount { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemCount { get; set; }
        public List<T> Items { get; set; }
    }
}
