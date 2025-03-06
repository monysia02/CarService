using CarService.Data;
using CarService.Model;
using Microsoft.EntityFrameworkCore;

namespace CarService.CustomerService;

public class CustomerService: ICustomerService
{
    private readonly ApplicationDbContext _conext;
    
    public CustomerService(ApplicationDbContext context)
    {
        _conext = context;
    }
    
    public async Task AddCustomerAsync(Customer customer)
    {
        await _conext.Customers.AddAsync(customer);
        await _conext.SaveChangesAsync();
    }
    
    public async Task<Customer> GetCustomerAsync(Guid id)
    {
        return await _conext.Customers.FindAsync(id);
    }
    
    public async Task<IEnumerable<Customer>> GetCustomersAsync()
    {
        return await _conext.Customers.ToListAsync();
    }
    
    public async Task UpdateCustomerAsync(Customer customer)
    {
        _conext.Customers.Update(customer);
        await _conext.SaveChangesAsync();
    }

}