using CarService.CarService;
using CarService.Data;
using CarService.DTOs.CarDto;
using CarService.DTOs.CustomerDto;
using CarService.Model;
using Microsoft.EntityFrameworkCore;

namespace CarService.Services
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _context;

        public CarService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCarAsync(CreateCarDto carDto)
        {
            var createdCarGuid = Guid.NewGuid();    
            
            var car = new Car
            {
                Id = createdCarGuid,
                Brand = carDto.Brand,
                Model = carDto.Model,
                Year = carDto.Year,
                Vin = carDto.Vin,
                RegistrationNumber = carDto.RegistrationNumber,
                CarCustomers = carDto.CustomerIds.Select(customerId => new CarCustomer
                {
                    CustomerId = customerId,
                    CarId = createdCarGuid
                }).ToList()
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
        }

        public async Task<ReadCarDto> GetCarAsync(Guid id)
        {
            var car = await _context.Cars
                .Include(c => c.CarCustomers)
                    .ThenInclude(cc => cc.Customer)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (car == null)
            {
                throw new Exception("Car not found");
            }

            return new ReadCarDto
            {
                CarId = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Vin = car.Vin,
                RegistrationNumber = car.RegistrationNumber,
                Customers = car.CarCustomers.Select(cc => new ReadCustomerDto
                {
                    CustomerId = cc.Customer.Id,
                    Name = cc.Customer.Name,
                    SurName = cc.Customer.SurName,
                    PhoneNumber = cc.Customer.PhoneNumber
                }).ToList()
            };
        }

        public async Task<IEnumerable<ReadCarDto>> GetCarsAsync()
        {
            var cars = await _context.Cars
                .Include(c => c.CarCustomers)
                    .ThenInclude(cc => cc.Customer)
                .ToListAsync();

            return cars.Select(car => new ReadCarDto
            {
                CarId = car.Id,
                Brand = car.Brand,
                Model = car.Model,
                Year = car.Year,
                Vin = car.Vin,
                RegistrationNumber = car.RegistrationNumber,
                Customers = car.CarCustomers.Select(cc => new ReadCustomerDto
                {
                    CustomerId = cc.Customer.Id,
                    Name = cc.Customer.Name,
                    SurName = cc.Customer.SurName,
                    PhoneNumber = cc.Customer.PhoneNumber
                }).ToList()
            });
        }

        public async Task UpdateCarAsync(UpdateCarDto updateCarDto)
        {
            var car = await _context.Cars.FindAsync(updateCarDto.CarId);
            if (car == null)
            {
                throw new Exception("Car not found");
            }
            car.Brand = updateCarDto.Brand;
            car.Model = updateCarDto.Model;
            car.Year = updateCarDto.Year;
            car.Vin = updateCarDto.Vin;
            car.RegistrationNumber = updateCarDto.RegistrationNumber;
            await _context.SaveChangesAsync();
        }
    }
}