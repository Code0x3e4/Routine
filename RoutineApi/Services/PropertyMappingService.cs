using RoutineApi.Entities;
using RoutineApi.Models;

namespace RoutineApi.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        private readonly Dictionary<string, PropertyMappingValue> _employeePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                { "Id",new PropertyMappingValue(new List<string> {"Id"}) },
                { "CompanyId",new PropertyMappingValue(new List<string> {"CompanyId"}) },
                { "EmployeeNo",new PropertyMappingValue(new List<string> {"EmployeeNo"})},
                { "Name",new PropertyMappingValue(new List<string> {"FirstName","LastName"})},
                { "GenderDisplay",new PropertyMappingValue(new List<string> {"Gender"}) },
                { "Age",new PropertyMappingValue(new List<string> {"DateOfBirth"}) },
            };

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto, Employee>(_employeePropertyMapping));
        }


        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMapping.Count() == 1)
                return matchingMapping.First()._mappingDictionary;

            throw new Exception($"无法找到唯一的映射关系：{typeof(TSource)},{typeof(TDestination)}");
        }

    }
}
