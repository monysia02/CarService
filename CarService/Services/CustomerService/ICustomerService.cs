using CarService.DTOs.CustomerDto;
using CarService.Model;
using Sieve.Models;

namespace CarService.CustomerService;

public interface ICustomerService
{
    Task AddCustomerAsync(CreateCustomerDto customer);
    Task<ReadCustomerDto> GetCustomerAsync(Guid id);
    Task<PaginatedResponse<ReadCustomerDto>> GetCustomersAsync(SieveModel sieveModel);
    Task UpdateCustomerAsync(UpdateCustomerDto customer);
}