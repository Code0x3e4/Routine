using Microsoft.EntityFrameworkCore;

namespace RoutineApi.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCout { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPage;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCout = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPage = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        //public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        //{
        //    var count = await source.CountAsync();
        //    var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        //    return new PagedList<T>(items, count, pageNumber, pageSize);
        //}
    }

}
