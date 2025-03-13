namespace CarService.DTOs.CustomerDto;

public class ReadCustomerDto
{
    public Guid CustomerId { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string PhoneNumber { get; set; }
}