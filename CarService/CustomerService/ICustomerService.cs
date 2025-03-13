using CarService.DTOs.CustomerDto;
using CarService.Model;

namespace CarService.CustomerService;

public interface ICustomerService
{
    Task AddCustomerAsync(CreateCustomerDto customer);
    Task<Customer> GetCustomerAsync(Guid id);
    Task<IEnumerable<Customer>> GetCustomersAsync();
    Task UpdateCustomerAsync(Customer customer);
}