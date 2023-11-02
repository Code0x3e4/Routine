using System.Reflection;

namespace RoutineApi.Services
{
    public class PropertyCheckerServic : IPropertyCheckerServic
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
                return true;

            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreReturn | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                    return false;
            }
            return true;
        }
    }
}
