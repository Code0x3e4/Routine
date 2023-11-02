using System.Dynamic;
using System.Reflection;

namespace RoutineApi.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var result = new List<ExpandoObject>(source.Count());

            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldSplit = fields.Split(',');
                foreach (var field in fieldSplit)
                {
                    var propertyName = field.Trim();
                    var propertyInfo = typeof(TSource).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance) 
                        ?? throw new Exception($"PropertyName：{propertyName} 没有找到：{typeof(TSource)}");
                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (var obj in source)
            {
                var shapedObj = new ExpandoObject();

                foreach (var i in propertyInfoList)
                {
                    var propertyValue = i.GetValue(obj);
                    ((IDictionary<string, object>)shapedObj).Add(i.Name, propertyValue);
                }
                result.Add(shapedObj);
            }
            return result;
        }
    }
}
