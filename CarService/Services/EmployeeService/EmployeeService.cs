using CarService.Data;
using CarService.DTOs.EmployeeDto;
using CarService.Model;
using CarService.Services.EmployeeService;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace CarService.EmployeeService;

public class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;
    private readonly SieveProcessor _sieveProcessor;

    public EmployeeService(ApplicationDbContext context, SieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public async Task AddEmployeeAsync(CreateEmployeeDto Employee)
    {
        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            Name = Employee.Name,
            SurName = Employee.SurName,
            PhoneNumber = Employee.PhoneNumber,
            Position = Employee.Position,
            RepairEmployees = new List<RepairEmployee>()
        };
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
    }

    public async Task<ReadEmployeeDto> GetEmployeeAsync(Guid id)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null)
        {
            throw new Exception("Employee not found");
        }

        return new ReadEmployeeDto
        {
            EmployeeId = employee.Id,
            Name = employee.Name,
            SurName = employee.SurName,
            PhoneNumber = employee.PhoneNumber,
            Position = employee.Position,
        };
    }

    public async Task<IEnumerable<ReadEmployeeDto>> GetEmployeesAsync(SieveModel sieveModel)
    {
        var query = _context.Employees.AsQueryable();
        query = _sieveProcessor.Apply(sieveModel, query);
        var employees = await query.ToListAsync();
        return employees.Select(employee => new ReadEmployeeDto
        {
            EmployeeId = employee.Id,
            Name = employee.Name,
            SurName = employee.SurName,
            PhoneNumber = employee.PhoneNumber,
            Position = employee.Position,
        });
    }

    public async Task UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto)
    {
        var employee = await _context.Employees.FindAsync(updateEmployeeDto.EmployeeId);
        if (employee == null)
        {
            throw new Exception("Employee not found");
        }
        employee.Name = updateEmployeeDto.Name;
        employee.SurName = updateEmployeeDto.SurName;
        employee.PhoneNumber = updateEmployeeDto.PhoneNumber;
        employee.Position = updateEmployeeDto.Position;
        await _context.SaveChangesAsync();
    }
}