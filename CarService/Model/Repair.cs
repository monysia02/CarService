using CarService.Enums;
using Sieve.Attributes;

namespace CarService.Model;

public class Repair
{
    public Guid Id { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime CreatedAt { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public DateTime? FinishedAt { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public Guid CarId { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public Car Car { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public string Description { get; set; }
    public RepairStatusEnum Status { get; set; }
    [Sieve(CanFilter = true, CanSort = true)]
    public decimal Price { get; set; }
    
    public ICollection<RepairEmployee> RepairEmployees { get; set; }

}