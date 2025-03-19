using CarService.Enums;

namespace CarService.DTOs.RepairDto;

public class UpdateRepairDto
{
    public Guid RepairId { get; set; }
    public Guid CarId { get; set; }
    public List<Guid> EmployeeIds { get; set; } = new List<Guid>(); 
    public string Description { get; set; } 
    public RepairStatusEnum Status { get; set; } 
    public decimal Price { get; set; } 
    public DateTime? FinishedAt { get; set; }}