using AutoMapper;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using RoutineApi.Entities;
using RoutineApi.Models;
using RoutineApi.Services;

namespace RoutineApi.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ICompanyRepository repoCompany;

        public EmployeesController(IMapper mapper, ICompanyRepository companyRepo)
        {
            this.mapper = mapper;
            this.repoCompany = companyRepo;
        }


        [HttpGet]
        //[ResponseCache(Duration = 120)]
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeDtoParam param)
        {
            if (!await repoCompany.CompanyExistsAsync(companyId))
                return NotFound();

            var employees = await repoCompany.GetEmployeesAsync(companyId, param);

            return Ok(mapper.Map<IEnumerable<EmployeeDto>>(employees));
        }

        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 60)]
        [HttpCacheValidation(MustRevalidate = false)]
        [HttpGet("{employeeId}", Name = nameof(GetEmployeeForCompany))]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompany(Guid companyId, Guid employeeId)
        {
            if (!await repoCompany.CompanyExistsAsync(companyId))
                return NotFound();

            var employee = await repoCompany.GetEmployeeByIdAsync(companyId, employeeId);
            if (employee == null)
                return NotFound();

            return Ok(mapper.Map<EmployeeDto>(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeDto>> CreateEmployeeForCompany(Guid companyId, EmployeeAddDto employee)
        {
            if (!await repoCompany.CompanyExistsAsync(companyId))
                return NotFound(companyId);

            var entity = mapper.Map<Employee>(employee);

            await repoCompany.AddEmployeeAsync(companyId, entity);
            await repoCompany.SaveAsync();

            var result = mapper.Map<EmployeeDto>(entity);
            return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, employeeId = result.Id }, result);
        }

        [HttpPut("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeUpdateDto employee)
        {
            return BadRequest();
        }

        [HttpPatch("{employeeId}")]
        public async Task<IActionResult> PartialUpdateEmployee(Guid companyId, Guid employeeId, JsonPatchDocument<EmployeeUpdateDto> JPDemployee)
        {
            if (!await repoCompany.CompanyExistsAsync(companyId))
                return NotFound();

            var employeeEntity = await repoCompany.GetEmployeeByIdAsync(employeeId, companyId);

            if (employeeEntity == null)
            {
                var employeeDto = new EmployeeUpdateDto();
                JPDemployee.ApplyTo(employeeDto, ModelState);

                if (!TryValidateModel(employeeDto))
                    return ValidationProblem(ModelState);

                var employeeToAdd = mapper.Map<Employee>(employeeDto);
                employeeToAdd.Id = employeeId;

                await repoCompany.AddEmployeeAsync(companyId, employeeToAdd);
                await repoCompany.SaveAsync();

                var result = mapper.Map<EmployeeDto>(employeeToAdd);
                return CreatedAtRoute(nameof(GetEmployeeForCompany), new { companyId, employeeId = result.Id }, result);
            }

            var dtoToPatch = mapper.Map<EmployeeUpdateDto>(employeeEntity);

            JPDemployee.ApplyTo(dtoToPatch, ModelState);

            if (!TryValidateModel(dtoToPatch))
                return ValidationProblem(ModelState);

            mapper.Map(dtoToPatch, employeeEntity);
            await repoCompany.UpdateEmployeeAsync(employeeEntity);
            await repoCompany.SaveAsync();
            return NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid employeeId)
        {
            if (!await repoCompany.CompanyExistsAsync(companyId))
                return NotFound();

            var employeeEntity = await repoCompany.GetEmployeeByIdAsync(employeeId, companyId);

            if (employeeEntity == null)
            {
                return NotFound();
            }

            repoCompany.DeleteEmployeeById(employeeId);
            await repoCompany.SaveAsync();
            return NoContent();
        }

        public override ActionResult ValidationProblem(ModelStateDictionary pairs)
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
            //return base.ValidationProblem();
        }
    }
}
