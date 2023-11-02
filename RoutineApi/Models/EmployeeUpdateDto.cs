using RoutineApi.Entities;
using System.ComponentModel.DataAnnotations;

namespace RoutineApi.Models
{
    public class EmployeeUpdateDto : IValidatableObject
    {
        public string EmployeeNo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                yield return new ValidationResult("姓与名不可相同", new[] { nameof(FirstName), nameof(LastName) });
            }
        }
    }
}
