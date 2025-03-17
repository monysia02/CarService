using CarService.Data;
using CarService.DTOs.CarDto;
using CarService.Model;
using Microsoft.EntityFrameworkCore;

namespace CarService.Services.CarService;

public class CarService : ICarService
{
    private readonly ApplicationDbContext _conext;

    public CarService(ApplicationDbContext context)
    {
        _conext = context;
    }

    public async Task AddCarAsync(CreateCarDto Car)
    {
        var car = new Car
        {
            Id = Guid.NewGuid(),
            Brand = Car.Brand,
            Model = Car.Model,
            Year = Car.Year,
            Vin = Car.Vin,
            RegistrationNumber = Car.RegistrationNumber,
            CarCustomers = new List<CarCustomer>()
        };
    }

    public async Task<ReadCarDto> GetCarAsync(Guid id)
    {
        var car = await _conext.Cars.FindAsync(id);
        if (car == null)
        {
            throw new Exception("Car not found");
        }

        return new ReadCarDto()
        {
            CarId = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            Vin = car.Vin,
            RegistrationNumber = car.RegistrationNumber
        };
    }

    public async Task<IEnumerable<ReadCarDto>> GetCarsAsync()
    {
        var cars = await _conext.Cars.ToListAsync();
        return cars.Select(car => new ReadCarDto
        {
            CarId = car.Id,
            Brand = car.Brand,
            Model = car.Model,
            Year = car.Year,
            Vin = car.Vin,
            RegistrationNumber = car.RegistrationNumber
        });
    }

    public async Task UpdateCarAsync(UpdateCarDto updateCarDto)
    {
        var car = await _conext.Cars.FindAsync(updateCarDto.CarId);
        if (car == null)
        {
            throw new Exception("Car not found");
        }
        car.Brand = updateCarDto.Brand;
        car.Model = updateCarDto.Model;
        car.Year = updateCarDto.Year;
        car.Vin = updateCarDto.Vin;
        car.RegistrationNumber = updateCarDto.RegistrationNumber;
        await _conext.SaveChangesAsync();
    }
}