namespace CarService.DTOs.RepairDto;

public class CreateRepairDto
{
    public Guid CarId { get; set; }
    public List<Guid> EmployeeIds { get; set; } = new();
    public string Description { get; set; }
}