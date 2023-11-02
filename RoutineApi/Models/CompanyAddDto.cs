using System.ComponentModel.DataAnnotations;

namespace RoutineApi.Models
{
    public class CompanyAddDto
    {
        [Display(Name = "公司名称")]
        [Required(ErrorMessage = "{0}这个字段是必填的")]
        [MaxLength(100,ErrorMessage = "{0}的最大长度不能超过{1}")]
        public string Name { get; set; }

        [Display(Name = "公司简介")]
        [StringLength(500, MinimumLength = 6,ErrorMessage = "{0}的长度范围从是{2}到{1}")]
        public string Introduction { get; set; }

        public ICollection<EmployeeAddDto> Employees { get; set; } = new List<EmployeeAddDto>();
    }
}
