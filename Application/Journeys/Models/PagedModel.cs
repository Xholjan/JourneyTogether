namespace Application.Journeys.Models
{
    public class PagedModel<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public PagedModel(IEnumerable<T> source, int currentPage, int pageSize)
        {
            if (currentPage < 1) currentPage = 1;
            if (pageSize < 1) pageSize = 10;

            TotalItems = source.Count();
            CurrentPage = currentPage;
            PageSize = pageSize;

            Items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
        }
    }
}
