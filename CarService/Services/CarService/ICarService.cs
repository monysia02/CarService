using CarService.DTOs.CarDto;

namespace CarService.Services.CarService;

public interface ICarService
{
    Task AddCarAsync(CreateCarDto car);
    Task<ReadCarDto> GetCarAsync(Guid id);
    Task<IEnumerable<ReadCarDto>> GetCarsAsync();
    Task UpdateCarAsync(UpdateCarDto car);
}