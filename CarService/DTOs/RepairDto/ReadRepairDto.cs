using CarService.DTOs.CarDto;
using CarService.DTOs.EmployeeDto;
using CarService.Enums;
using CarService.Model;

namespace CarService.DTOs.RepairDto;

public class ReadRepairDto
{
    public Guid RepairId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public ReadCarDto Car { get; set; }
    public string Description { get; set; }
    public RepairStatusEnum Status { get; set; }
    public decimal Price { get; set; }
    public List<ReadEmployeeDto> Employees { get; set; } = new List<ReadEmployeeDto>();
}