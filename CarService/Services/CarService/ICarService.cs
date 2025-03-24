using CarService.DTOs.CarDto;
using CarService.Model;
using Sieve.Models;

namespace CarService.CarService;

public interface ICarService
{
    Task AddCarAsync(CreateCarDto car);
    Task<ReadCarDto> GetCarAsync(Guid id);
    Task<PaginatedResponse<ReadCarDto>> GetCarsAsync(SieveModel  sieveModel);
    Task UpdateCarAsync(UpdateCarDto car);
}