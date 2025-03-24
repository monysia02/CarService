using Sieve.Attributes;

namespace CarService.Model;

public class Customer
{
    public Guid Id { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string Name { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string SurName { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string PhoneNumber { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    
    public ICollection<CarCustomer> CarCustomers { get; set; }
}