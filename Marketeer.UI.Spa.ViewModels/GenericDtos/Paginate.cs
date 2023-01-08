using Marketeer.Core.Domain.Dtos;

namespace Marketeer.UI.Spa.ViewModels.GenericDtos
{
    public class Paginate : PaginateDto
    {
    }

    public class PaginateGeneric<T> : PaginateDto<T> where T : class
    {
    }
}
