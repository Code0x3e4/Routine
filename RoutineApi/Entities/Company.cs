using System.ComponentModel.DataAnnotations;

namespace RoutineApi.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; }
        [Required,MaxLength(500)]
        public string Introduction { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
