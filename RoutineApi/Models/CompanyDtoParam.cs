namespace RoutineApi.Models
{
    public class CompanyDtoParam
    {
        private const int MaxPageSize = 50;
        private int pageSize = 10;

        public string CompanyName { get; set; }

        public string SearchTerm { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get => pageSize; set => pageSize = (value > MaxPageSize) ? MaxPageSize : value; }

        public string OrderBy { get; set; } = "CompanyName";

        public string Fields { get; set; }
    }
}
