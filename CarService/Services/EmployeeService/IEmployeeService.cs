using CarService.DTOs.EmployeeDto;
using Sieve.Models;

namespace CarService.Services.EmployeeService;

public interface IEmployeeService
{
    Task AddEmployeeAsync(CreateEmployeeDto Employee);
    Task<ReadEmployeeDto> GetEmployeeAsync(Guid id);
    Task<IEnumerable<ReadEmployeeDto>> GetEmployeesAsync(SieveModel sieveModel);
    Task UpdateEmployeeAsync(UpdateEmployeeDto Employee);
}