using RoutineApi.Entities;
using RoutineApi.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace RoutineApi.Models
{
    [EmployeeNoLimit(ErrorMessage = "员工编号不可以与名字相同")]
    public class EmployeeAddDto : IValidatableObject
    {
        public string EmployeeNo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(FirstName == LastName)
            {
                yield return new ValidationResult("姓与名不可相同", new[] { nameof(FirstName), nameof(LastName) });
            }
        }
    }
}
