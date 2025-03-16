using CarService.DTOs.EmployeeDto;

namespace CarService.Services.EmployeeService;

public interface IEmployeeService
{
    Task AddEmployeeAsync(CreateEmployeeDto Employee);
    Task<ReadEmployeeDto> GetEmployeeAsync(Guid id);
    Task<IEnumerable<ReadEmployeeDto>> GetEmployeesAsync();
    Task UpdateEmployeeAsync(UpdateEmployeeDto Employee);
}