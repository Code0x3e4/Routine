using System.Dynamic;
using System.Reflection;

namespace RoutineApi.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<T>(this T source, string fields)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            ExpandoObject expandoObj = new();
            if(string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(T).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                foreach( var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string,object>) expandoObj).Add(propertyInfo.Name, propertyValue);
                }
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');
                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                        ?? throw new Exception($"在：{typeof(T)}上没有找到：{propertyName}");

                    var propertyValue = propertyInfo.GetValue(source);
                    ((IDictionary<string, object>)expandoObj).Add(propertyInfo.Name, propertyValue);
                }
            }
            return expandoObj;
        }
        
    }
}
