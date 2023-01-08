namespace Marketeer.Core.Domain.Dtos
{
    // RefactorTypes are in ViewModels.GenericDtos
    public class PaginateDto
    {
        public int PageIndex { get; set; }
        public int PageItemCount { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemCount { get; set; }
    }

    // Map is defined in Common.Mapper.MappingProfile
    public class PaginateDto<T> : PaginateDto where T : class
    {
        public List<T> Items { get; set; }
    }
}
