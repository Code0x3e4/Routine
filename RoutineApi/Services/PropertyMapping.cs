namespace RoutineApi.Services
{
    public class PropertyMapping<TSource,TDestination> : IPropertyMapping
    {
        public Dictionary<string, PropertyMappingValue> _mappingDictionary {  get; set; }

        public PropertyMapping(Dictionary<string, PropertyMappingValue> pairs)
        {
            _mappingDictionary = pairs;
        }


    }
}
