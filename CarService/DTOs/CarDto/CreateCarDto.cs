namespace CarService.DTOs.CarDto;

public class CreateCarDto
{
    public List<Guid> CustomerIds { get; set; } = new List<Guid>();
    public string Brand { get; set; }
    public string Model { get; set; }
    public string RegistrationNumber { get; set; }
    public string Vin { get; set; }
    public int Year { get; set; }
}