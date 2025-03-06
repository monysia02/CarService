namespace CarService.Model;

public class RepairEmployee
{
    public Guid RepairId { get; set; }
    public Repair Repair { get; set; }
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; }
}