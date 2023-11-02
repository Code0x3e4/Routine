namespace RoutineApi.Services
{
    public interface IPropertyCheckerServic
    {
        bool TypeHasProperties<T>(string fields);
    }
}