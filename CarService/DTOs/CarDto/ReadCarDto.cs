using CarService.DTOs.CustomerDto;

namespace CarService.DTOs.CarDto
{
    public class ReadCarDto
    {
        public Guid CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public string Vin { get; set; }
        public int Year { get; set; }
        public List<ReadCustomerDto> Customers { get; set; } = new List<ReadCustomerDto>();
    }
}