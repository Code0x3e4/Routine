using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoutineApi.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }

        public Guid CompanyId { get; set; }

        [Required,MaxLength(10)]
        public string EmployeeNo { get; set; }

        [Required,MaxLength(50)]
        public string FirestName { get; set; }

        [Required,MaxLength(50)]
        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Company Company { get; set; }
    }

    public enum Gender
    {
        male = 1, 
        female = 2
    }
}