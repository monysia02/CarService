using CarService.DTOs.CustomerDto;
using CarService.Model;

namespace CarService.CustomerService;

public interface ICustomerService
{
    Task AddCustomerAsync(CreateCustomerDto customer);
    Task<ReadCustomerDto> GetCustomerAsync(Guid id);
    Task<IEnumerable<ReadCustomerDto>> GetCustomersAsync();
    Task UpdateCustomerAsync(UpdateCustomerDto customer);
}