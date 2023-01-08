using System.ComponentModel.DataAnnotations;

namespace Marketeer.Core.Domain.Dtos
{
    public class PaginateFilterDto : IRefactorType
    {
        public int PageIndex { get; set; }
        [Range(1, int.MaxValue)]
        public int PageItemCount { get; set; }
        public string? OrderBy { get; set; }
        public bool IsOrderAsc { get; set; }
        public bool IsPaginated { get; set; } = true;
    }

    public class PaginateFilterDto<T> : PaginateFilterDto where T : class
    {
        public T Filter { get; set; }
    }
}
