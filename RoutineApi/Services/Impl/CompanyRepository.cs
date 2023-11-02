using Microsoft.EntityFrameworkCore;
using RoutineApi.Data;
using RoutineApi.Entities;
using RoutineApi.Helpers;
using RoutineApi.Models;

namespace RoutineApi.Services.Impl
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly RoutineDbContext _contex;
        private readonly IPropertyMappingService mappingService;

        public CompanyRepository(RoutineDbContext context,IPropertyMappingService mappingService)
        {
            _contex = context ?? throw new ArgumentException(null, nameof(context));
            this.mappingService = mappingService;
        }
        public async Task AddCompanyAsync(Company company)
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            company.Id = Guid.NewGuid();

            foreach (var employee in company.Employees)
            {
                employee.Id = Guid.NewGuid();
            }

            await _contex.Companies.AddAsync(company);
        }

        public async Task AddEmployeeAsync(Guid companyId, Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            if (companyId == Guid.Empty)
                throw new ArgumentNullException(nameof(companyId));

            employee.CompanyId = companyId;
            await _contex.Employees.AddAsync(employee);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentNullException(nameof(companyId));

            return await _contex.Companies.AnyAsync(x => x.Id == companyId);
        }

        public void DeleteCompany(Guid companyId)
        {
            if (companyId == Guid.Empty)
                return;

            _contex.Companies.Remove(new Company { Id = companyId });
        }

        public void DeleteEmployeeById(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
                return;

            _contex.Employees.Remove(new Employee { Id = employeeId });
        }

        public async Task<bool> EmployeeExistsAsync(Guid employeeId)
        {
            if (employeeId == Guid.Empty)
                throw new ArgumentNullException(nameof(employeeId));

            return await _contex.Employees.AnyAsync(x => x.Id == employeeId);
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> ids)
        {
            if (ids.Any())
            {
                return await _contex.Companies.Where(x => ids.Contains(x.Id)).ToListAsync();
            }
            else
                return await GetCompaniesAsync(new CompanyDtoParam());
        }
        public async Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParam param)
        {
            if(param == null)
                throw new ArgumentNullException(nameof(param));


            var query = _contex.Companies as IQueryable<Company>;
            if(!string.IsNullOrWhiteSpace(param.CompanyName))
            {
                param.CompanyName = param.CompanyName.Trim();
                query = query.Where(x => x.Name == param.CompanyName);
            }

            if(!string.IsNullOrWhiteSpace(param.SearchTerm))
            {
                param.SearchTerm = param.SearchTerm.Trim();
                query = query.Where(x => x.Name.Contains(param.SearchTerm) || x.Introduction.Contains(param.SearchTerm));
            }

            return await query.ToPagedList(param.PageNumber, param.PageSize);
        }

        public async Task<IEnumerable<Company>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
                throw new ArgumentNullException(nameof(companyIds));

            return await _contex.Companies.Where(x => companyIds.Contains(x.Id))
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException(nameof(id));

            return await _contex.Companies.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, Guid companyId)
        {
            if (employeeId == Guid.Empty)
                throw new ArgumentNullException(nameof(employeeId));
            if (companyId == Guid.Empty)
                throw new ArgumentNullException(nameof(companyId));

            return await _contex.Employees
                .Where(x => x.CompanyId == companyId && x.Id == employeeId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, EmployeeDtoParam param)
        {
            if (companyId == Guid.Empty)
                throw new ArgumentNullException(nameof(companyId));

            var items = _contex.Employees.Where(x => x.CompanyId == companyId);

            if(!string.IsNullOrWhiteSpace(param.Gender))
            {
                param.Gender = param.Gender.Trim();
                var gender = Enum.Parse<Gender>(param.Gender);

                items = items.Where(x => x.Gender == gender);
            }
            
            if(!string.IsNullOrWhiteSpace(param.Q))
            {
                param.Q = param.Q.Trim();

                items = items.Where(x => 
                    x.EmployeeNo.Contains(param.Q) || x.FirestName.Contains(param.Q) || x.LastName.Contains(param.Q));
            }

            var mapppingDictionary = mappingService.GetPropertyMapping<EmployeeDto, Employee>();


            return await items.ApplySort(param.OrderBy, mapppingDictionary).ToListAsync();

        }

        public async Task<bool> SaveAsync()
        {
            return await _contex.SaveChangesAsync() >= 0;
        }

        public Task UpdateCompanyAsync(Company company)
        {
            // 一般为局部更新，需要根据需求调整更新逻辑
            _contex.Entry(company).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task UpdateEmployeeAsync(Employee employee)
        {
            _contex.Entry(employee).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
