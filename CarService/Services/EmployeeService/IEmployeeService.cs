using CarService.DTOs.EmployeeDto;
using CarService.Model;
using Sieve.Models;

namespace CarService.Services.EmployeeService;

public interface IEmployeeService
{
    Task AddEmployeeAsync(CreateEmployeeDto Employee);
    Task<ReadEmployeeDto> GetEmployeeAsync(Guid id);
    Task<PaginatedResponse<ReadEmployeeDto>> GetEmployeesAsync(SieveModel sieveModel);
    Task UpdateEmployeeAsync(UpdateEmployeeDto Employee);
}