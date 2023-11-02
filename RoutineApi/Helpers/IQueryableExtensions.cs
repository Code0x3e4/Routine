using Microsoft.EntityFrameworkCore;
using RoutineApi.Services;
using System.Linq.Dynamic.Core;

namespace RoutineApi.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> pairs)
        {

            var orderByAfterSplit = orderBy.Split(',');

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var item = orderByClause.Trim();

                var orderDesc = item.EndsWith(" desc");

                var indexOfFirstSpace = item.IndexOf(" ");

                // 单属性排序
                var propertyName = indexOfFirstSpace == -1 ? item : item.Remove(indexOfFirstSpace);

                if (!pairs.ContainsKey(propertyName))
                    throw new ArgumentNullException($"没有找到Key为{propertyName}的映射");

                var mappingValue = pairs[propertyName];
                if (mappingValue == null)
                    throw new ArgumentNullException(nameof(mappingValue));


                foreach (var property in mappingValue.DestinationProperties)
                {
                    if (mappingValue.Revert)
                        orderDesc = !orderDesc;

                    source = source.OrderBy(property + (orderDesc ? " descending" : " ascending"));
                }
            }

            return source;
        }


        public static async Task<PagedList<T>> ToPagedList<T>(this IQueryable<T> q, int pageNumber, int pageSize)
        {
            var count = await q.CountAsync();
            var items = await q.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }

}
