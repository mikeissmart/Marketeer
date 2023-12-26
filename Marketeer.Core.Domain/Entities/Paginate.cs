using Marketeer.Core.Domain.Dtos;

namespace Marketeer.Core.Domain
{
    public class Paginate<T>
    {
        public int PageIndex { get; set; }
        public int PageItemCount { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
