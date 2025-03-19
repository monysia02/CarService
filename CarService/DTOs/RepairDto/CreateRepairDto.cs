using CarService.Enums;

namespace CarService.DTOs.RepairDto;

public class CreateRepairDto
{
    public Guid CarId { get; set; }
    public List<Guid> EmployeeIds { get; set; } = new List<Guid>(); 
    public string Description { get; set; }

    public RepairStatusEnum Status { get; set; } = RepairStatusEnum.New;
    //public decimal Price { get; set; } //?
    // public DateTime FinishedAt { get; set; } 
}