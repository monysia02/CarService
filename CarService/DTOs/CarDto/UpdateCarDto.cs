namespace CarService.DTOs.CarDto;

public class UpdateCarDto
{
    public Guid CarId { get; set; }
    public List<Guid> CustomerId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public string RegistrationNumber { get; set; }
    public string Vin { get; set; }
    public int Year { get; set; }
}