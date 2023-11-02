using RoutineApi.Entities;
using RoutineApi.Helpers;
using RoutineApi.Models;

namespace RoutineApi.Services
{
    public interface ICompanyRepository
    {
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> ids);
        Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParam param);
        Task<Company> GetCompanyByIdAsync(Guid id);
        Task<IEnumerable<Company>> GetCompaniesByIdsAsync(IEnumerable<Guid> companyIds);
        Task AddCompanyAsync(Company company);
        Task UpdateCompanyAsync(Company company);
        void DeleteCompany(Guid companyId);
        Task<bool> CompanyExistsAsync(Guid companyId);

        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, EmployeeDtoParam param);
        Task<Employee?> GetEmployeeByIdAsync(Guid employeeId, Guid companyId);
        Task AddEmployeeAsync(Guid companyId, Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        void DeleteEmployeeById(Guid employeeId);
        Task<bool> EmployeeExistsAsync(Guid employeeId);

        Task<bool> SaveAsync();

    }
}
