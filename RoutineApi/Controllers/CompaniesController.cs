using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoutineApi.Entities;
using RoutineApi.Helpers;
using RoutineApi.Models;
using RoutineApi.Services;

namespace RoutineApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository repoCompany;
        private readonly ILogger<CompaniesController> logger;
        private readonly IMapper mapper;
        private readonly IPropertyCheckerServic propertyChecker;

        public CompaniesController(ICompanyRepository companyRepository, ILogger<CompaniesController> logger, 
            IMapper mapper,IPropertyCheckerServic propertyChecker)
        {
            repoCompany = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
            this.logger = logger;
            this.mapper = mapper;
            this.propertyChecker = propertyChecker;
        }

        [HttpGet(Name = nameof(GetCompanies))]
        [HttpHead]
        public async Task<IActionResult> GetCompanies([FromQuery] CompanyDtoParam param)
        {
            if(!propertyChecker.TypeHasProperties<CompanyDto>(param.Fields))
                return BadRequest();

            var companies = await repoCompany.GetCompaniesAsync(param);

            var previousPageLink = companies.HasPrevious ? CreateCompaniesResourceUri(param, ResourceUriType.PreviousPage) : null;
            var nextPageLink = companies.HasNext ? CreateCompaniesResourceUri(param, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = companies.Count,
                pageSize = companies.PageSize,
                currentPage = companies.CurrentPage,
                totalPage = companies.TotalPage,
                previousPageLink,
                nextPageLink
            };

            Response.Headers.Add("X-Pagination", System.Text.Json.JsonSerializer.Serialize(paginationMetadata,
                new System.Text.Json.JsonSerializerOptions
                {
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                }));

            var result = mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(result.ShapeData(param.Fields));
        }

        [HttpGet("{companyId}", Name = nameof(GetCompany))]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId, string fields)
        {
            if (!propertyChecker.TypeHasProperties<CompanyDto>(fields))
                return BadRequest();
            var result = repoCompany.GetCompanyByIdAsync(companyId);
            if (result == null)
                return NotFound();

            return Ok(mapper.Map<CompanyDto>(result.ShapeData(fields)));
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto company)
        {
            var entity = mapper.Map<Company>(company);

            await repoCompany.AddCompanyAsync(entity);

            await repoCompany.SaveAsync();

            var result = mapper.Map<CompanyDto>(entity);
            return CreatedAtRoute(nameof(GetCompany), new { companyId = result.Id }, result);
        }

        [HttpPost("{ids}")]
        public async Task<ActionResult<CompanyDto>> GetCompayCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<Guid> ids)
        {
            if (ids == null)
                return BadRequest();

            var entities = await repoCompany.GetCompaniesAsync(ids);

            if (ids.Count() != entities.Count())
                return NotFound();

            return Ok(mapper.Map<IEnumerable<CompanyDto>>(entities));
        }

        [HttpDelete("companyId")]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var entity = await repoCompany.GetCompanyByIdAsync(companyId);
            if (entity == null)
                return NotFound();

            repoCompany.DeleteCompany(companyId);
            await repoCompany.SaveAsync();

            return NoContent();
        }


        private string CreateCompaniesResourceUri(CompanyDtoParam param, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
#pragma warning disable CS8603 // 可能返回 null 引用。
                    return Url.Link(nameof(GetCompanies), new
                    {
                        fields = param.Fields,
                        orderBy = param.OrderBy,
                        pageNumber = param.PageNumber - 1,
                        pageSize = param.PageSize,
                        companyName = param.CompanyName,
                        searchTerm = param.SearchTerm
                    });
                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        fields = param.Fields,
                        orderBy = param.OrderBy,
                        pageNumber = param.PageNumber + 1,
                        pageSize = param.PageSize,
                        companyName = param.CompanyName,
                        searchTerm = param.SearchTerm
                    });
                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        fields = param.Fields,
                        orderBy = param.OrderBy,
                        pageNumber = param.PageNumber,
                        pageSize = param.PageSize,
                        companyName = param.CompanyName,
                        searchTerm = param.SearchTerm
                    });
#pragma warning restore CS8603 // 可能返回 null 引用。
            }
        }
    }
}
