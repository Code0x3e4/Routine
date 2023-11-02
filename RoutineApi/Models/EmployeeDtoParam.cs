namespace RoutineApi.Models
{
    public class EmployeeDtoParam
    {
        private const int MaxPageSize = 20;
        private int pageSize = 5;

        public string Gender { get; set; }
        public string Q { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string OrderBy { get; set; } = "Name";

    }
}
