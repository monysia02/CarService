using CarService.Data;
using CarService.DTOs.RepairDto;
using CarService.Enums;
using CarService.Model;
using CarService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace CarService.Tests.Services;

public class RepairServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly RepairService _repairService;
    private readonly SieveProcessor _sieveProcessor;

    public RepairServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        _sieveProcessor = new SieveProcessor(Options.Create(new SieveOptions()));

        _repairService = new RepairService(_context, _sieveProcessor);

        var car = new Car
        {
            Id = Guid.NewGuid(),
            Brand = "Toyota",
            Model = "Corolla",
            RegistrationNumber = "ABC123",
            Vin = "TESTVIN123",
            Year = 2020
        };
        _context.Cars.Add(car);

        var employee1 = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Jan",
            SurName = "Kowalski",
            PhoneNumber = "123456789",
            Position = "Mechanik"
        };

        var employee2 = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Anna",
            SurName = "Nowak",
            PhoneNumber = "987654321",
            Position = "Mechanik"
        };

        _context.Employees.AddRange(employee1, employee2);
        _context.SaveChanges();
    }

    [Fact]
    public async Task AddRepairAsync_ShouldAddRepair_WhenDataIsValid()
    {
        var carId = _context.Cars.First().Id;
        var employeeIds = _context.Employees.Select(e => e.Id).ToList();

        var createRepairDto = new CreateRepairDto
        {
            CarId = carId,
            Description = "Testowa naprawa",
            EmployeeIds = employeeIds
        };

        await _repairService.AddRepairAsync(createRepairDto);

        var repair = await _context.Repairs.Include(r => r.RepairEmployees).FirstOrDefaultAsync();
        Assert.NotNull(repair);
        Assert.Equal("Testowa naprawa", repair.Description);
        Assert.Equal(employeeIds.Count, repair.RepairEmployees.Count);
        Assert.Equal(RepairStatusEnum.New, repair.Status);
    }

    [Fact]
    public async Task AddRepairAsync_ShouldThrowException_WhenCarNotFound()
    {
        var nonExistentCarId = Guid.NewGuid();
        var employeeIds = _context.Employees.Select(e => e.Id).ToList();

        var createRepairDto = new CreateRepairDto
        {
            CarId = nonExistentCarId,
            Description = "Testowa naprawa",
            EmployeeIds = employeeIds
        };

        await Assert.ThrowsAsync<Exception>(() => _repairService.AddRepairAsync(createRepairDto));
    }

    [Fact]
    public async Task FinishRepairAsync_ShouldSetStatusAndFinalPrice()
    {
        var carId = _context.Cars.First().Id;
        var employeeIds = _context.Employees.Select(e => e.Id).ToList();
        var createRepairDto = new CreateRepairDto
        {
            CarId = carId,
            Description = "Naprawa do ukończenia",
            EmployeeIds = employeeIds
        };
        await _repairService.AddRepairAsync(createRepairDto);
        var repair = await _context.Repairs.FirstOrDefaultAsync();
        var finalPrice = 999.99m;

        await _repairService.FinishRepairAsync(repair.Id, finalPrice);

        var finishedRepair = await _context.Repairs.FirstOrDefaultAsync(r => r.Id == repair.Id);
        Assert.NotNull(finishedRepair);
        Assert.Equal(RepairStatusEnum.Finished, finishedRepair.Status);
        Assert.Equal(finalPrice, finishedRepair.Price);
        Assert.NotNull(finishedRepair.FinishedAt);
    }
}