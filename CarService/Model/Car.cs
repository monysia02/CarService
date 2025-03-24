using Sieve.Attributes;

namespace CarService.Model;

public class Car
{   
    public Guid Id { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string Brand { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string Model { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string RegistrationNumber { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string Vin { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public int Year { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public ICollection<CarCustomer> CarCustomers { get; set; } = new List<CarCustomer>();
}