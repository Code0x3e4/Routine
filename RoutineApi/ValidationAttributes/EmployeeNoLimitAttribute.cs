using RoutineApi.Models;
using System.ComponentModel.DataAnnotations;

namespace RoutineApi.ValidationAttributes
{
    public class EmployeeNoLimitAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var addDto = (EmployeeAddDto)validationContext.ObjectInstance;

            if(addDto.EmployeeNo == addDto.FirstName)
            {
                return new ValidationResult(ErrorMessage, new[] { nameof(EmployeeNoLimitAttribute) });
            }
            
            return ValidationResult.Success;
        }
    }
}
