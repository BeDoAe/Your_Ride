namespace Your_Ride.Helper
{
    public class PaginatedList<T>where T : class
    {
        public List<T> Items { get; private set; }
        public int TotalItems { get; private set; }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public PaginatedList(List<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}