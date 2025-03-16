namespace CarService.DTOs.EmployeeDto;

public class ReadEmployeeDto
{
    public Guid EmployeeId { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string PhoneNumber { get; set; }
    public string Position { get; set; }
}