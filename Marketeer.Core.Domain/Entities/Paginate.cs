namespace Marketeer.Core.Domain
{
    public class Paginate<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageItemCount { get; set; }
        public int TotalPages { get; set; }
        public int TotalItemCount { get; set; }
        public List<T> Items { get; set; }
    }
}
