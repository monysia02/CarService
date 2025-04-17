using CarService.Data;
using CarService.DTOs.CustomerDto;
using CarService.Model;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace CarService.CustomerService;

public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _conext;
    private readonly SieveProcessor _sieveProcessor;

    public CustomerService(ApplicationDbContext context, SieveProcessor sieveProcessor)
    {
        _conext = context;
        _sieveProcessor = sieveProcessor;
    }

    public async Task AddCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        var customer = new Customer
        {
            Id = Guid.NewGuid(),
            Name = createCustomerDto.Name,
            SurName = createCustomerDto.SurName,
            PhoneNumber = createCustomerDto.PhoneNumber,
            CarCustomers = new List<CarCustomer>()
        };

        await _conext.Customers.AddAsync(customer);
        await _conext.SaveChangesAsync();
    }

    public async Task<ReadCustomerDto> GetCustomerAsync(Guid id)
    {
        var customer = await _conext.Customers.FindAsync(id);
        if (customer == null) throw new Exception("Customer not found");

        return new ReadCustomerDto
        {
            CustomerId = customer.Id,
            Name = customer.Name,
            SurName = customer.SurName,
            PhoneNumber = customer.PhoneNumber
        };
    }

    public async Task<PaginatedResponse<ReadCustomerDto>> GetCustomersAsync(SieveModel sieveModel)
    {
        var query = _conext.Customers.AsQueryable();

        var totalCount = await query.CountAsync();

        query = _sieveProcessor.Apply(sieveModel, query);

        var customers = await query.ToListAsync();

        var customersDtos = customers.Select(customer => new ReadCustomerDto
        {
            CustomerId = customer.Id,
            Name = customer.Name,
            SurName = customer.SurName,
            PhoneNumber = customer.PhoneNumber
        });

        return new PaginatedResponse<ReadCustomerDto>
        {
            Data = customersDtos,
            CurrentPage = sieveModel.Page ?? 1,
            PageSize = sieveModel.PageSize ?? 10,
            TotalCount = totalCount
        };
    }

    public async Task UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _conext.Customers.FindAsync(updateCustomerDto.CustomerId);
        if (customer == null) throw new Exception("Customer not found");

        customer.Name = updateCustomerDto.Name;
        customer.SurName = updateCustomerDto.SurName;
        customer.PhoneNumber = updateCustomerDto.PhoneNumber;

        _conext.Customers.Update(customer);
        await _conext.SaveChangesAsync();
    }
}