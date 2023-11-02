namespace RoutineApi.Services
{
    public class PropertyMappingValue
    {
        public IEnumerable<string> DestinationProperties { get; set; }

        public bool Revert { get; set; }

        public PropertyMappingValue(IEnumerable<string> proper, bool revert = false)
        {
            Revert = revert;
            DestinationProperties = proper ?? throw new ArgumentNullException(nameof(proper));
        }
    }
}
