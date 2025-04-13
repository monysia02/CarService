using CarService.Data;
using CarService.DTOs.EmployeeDto;
using CarService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace CarService.Tests.Services;

public class EmployeeServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly EmployeeService.EmployeeService _employeeService;
    private readonly SieveProcessor _sieveProcessor;

    public EmployeeServiceTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);

        _sieveProcessor = new SieveProcessor(Options.Create(new SieveOptions()));

        _employeeService = new EmployeeService.EmployeeService(_context, _sieveProcessor);
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldAddEmployee_WhenDataIsValid()
    {
        var createEmployeeDto = new CreateEmployeeDto
        {
            Name = "Marek",
            SurName = "Nowak",
            PhoneNumber = "111222333",
            Position = "Mechanik"
        };

        await _employeeService.AddEmployeeAsync(createEmployeeDto);

        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Name == "Marek");
        Assert.NotNull(employee);
        Assert.Equal(createEmployeeDto.SurName, employee.SurName);
        Assert.Equal(createEmployeeDto.PhoneNumber, employee.PhoneNumber);
        Assert.Equal(createEmployeeDto.Position, employee.Position);
    }

    [Fact]
    public async Task GetEmployeeAsync_ShouldReturnEmployee_WhenExists()
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Anna",
            SurName = "Kowalska",
            PhoneNumber = "444555666",
            Position = "Specjalistka",
            RepairEmployees = Enumerable.Empty<RepairEmployee>().ToList()
        };
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        var result = await _employeeService.GetEmployeeAsync(employee.Id);

        Assert.NotNull(result);
        Assert.Equal(employee.Id, result.EmployeeId);
        Assert.Equal(employee.Name, result.Name);
    }

    [Fact]
    public async Task GetEmployeesAsync_ShouldReturnPaginatedList()
    {
        await _context.Employees.AddAsync(new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Adam",
            SurName = "Kowal",
            PhoneNumber = "123123123",
            Position = "Mechanik",
            RepairEmployees = Enumerable.Empty<RepairEmployee>().ToList()
        });
        await _context.Employees.AddAsync(new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Ewa",
            SurName = "Wiśniewska",
            PhoneNumber = "321321321",
            Position = "Specjalistka",
            RepairEmployees = Enumerable.Empty<RepairEmployee>().ToList()
        });
        await _context.SaveChangesAsync();

        var sieveModel = new SieveModel
        {
            Page = 1,
            PageSize = 10
        };

        var paginatedResponse = await _employeeService.GetEmployeesAsync(sieveModel);

        Assert.NotNull(paginatedResponse);
        Assert.True(paginatedResponse.TotalCount >= 2);
        Assert.True(paginatedResponse.Data.Any());
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ShouldUpdateEmployee_WhenEmployeeExists()
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = "Piotr",
            SurName = "Kowalski",
            PhoneNumber = "777888999",
            Position = "Mechanik",
            RepairEmployees = Enumerable.Empty<RepairEmployee>().ToList()
        };
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        var updateEmployeeDto = new UpdateEmployeeDto
        {
            EmployeeId = employee.Id,
            Name = "Piotr Zmodyfikowany",
            SurName = "Nowy Kowalski",
            PhoneNumber = "000111222",
            Position = "Główny Mechanik"
        };

        await _employeeService.UpdateEmployeeAsync(updateEmployeeDto);

        var updatedEmployee = await _context.Employees.FindAsync(employee.Id);
        Assert.NotNull(updatedEmployee);
        Assert.Equal(updateEmployeeDto.Name, updatedEmployee.Name);
        Assert.Equal(updateEmployeeDto.SurName, updatedEmployee.SurName);
        Assert.Equal(updateEmployeeDto.PhoneNumber, updatedEmployee.PhoneNumber);
        Assert.Equal(updateEmployeeDto.Position, updatedEmployee.Position);
    }
}