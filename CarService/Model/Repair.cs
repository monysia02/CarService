using CarService.Enums;

namespace CarService.Model;

public class Repair
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime FinishedAt { get; set; }
    public Guid CarId { get; set; }
    public List<Guid> EmployeeId { get; set; }
    public string Description { get; set; }
    public RepairStatusEnum Status { get; set; }
    public decimal Price { get; set; }
}