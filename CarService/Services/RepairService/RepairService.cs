using CarService.Data;
using CarService.DTOs.CarDto;
using CarService.DTOs.EmployeeDto;
using CarService.DTOs.RepairDto;
using CarService.Enums;
using CarService.Model;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace CarService.Services;

public class RepairService : IRepairService
{
    private readonly ApplicationDbContext _context;
    private readonly SieveProcessor _sieveProcessor;

    public RepairService(ApplicationDbContext context, SieveProcessor sieveProcessor)
    {
        _context = context;
        _sieveProcessor = sieveProcessor;
    }

    public async Task AddRepairAsync(CreateRepairDto repairDto)
    {
        var createdRepairGuid = Guid.NewGuid();

        var car = await _context.Cars.FindAsync(repairDto.CarId);

        if (car == null) throw new Exception("Car not found");

        var employees = await _context.Employees.Where(e => repairDto.EmployeeIds.Contains(e.Id)).ToListAsync();

        if (employees.Count != repairDto.EmployeeIds.Count) throw new Exception("Employee not found");

        var repair = new Repair
        {
            Id = createdRepairGuid,
            CreatedAt = DateTime.UtcNow,
            CarId = repairDto.CarId,
            Description = repairDto.Description,
            Status = RepairStatusEnum.New,
            RepairEmployees = repairDto.EmployeeIds.Select(employeeId => new RepairEmployee
            {
                RepairId = createdRepairGuid,
                EmployeeId = employeeId
            }).ToList()
        };

        _context.Repairs.Add(repair);
        await _context.SaveChangesAsync();
    }

    public async Task<ReadRepairDto> GetRepairAsync(Guid id)
    {
        var repair = await _context.Repairs
            .Include(r => r.Car)
            .Include(r => r.RepairEmployees)
            .ThenInclude(re => re.Employee)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (repair == null) throw new Exception("Repair not found");

        return new ReadRepairDto
        {
            RepairId = repair.Id,
            CreatedAt = repair.CreatedAt,
            FinishedAt = repair.FinishedAt,
            Car = repair.Car == null
                ? null
                : new ReadCarDto
                {
                    CarId = repair.Car.Id,
                    Brand = repair.Car.Brand,
                    Model = repair.Car.Model,
                    RegistrationNumber = repair.Car.RegistrationNumber,
                    Vin = repair.Car.Vin,
                    Year = repair.Car.Year
                },
            Description = repair.Description,
            Status = repair.Status,
            Price = repair.Price,
            Employees = repair.RepairEmployees.Select(e => new ReadEmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Employee.Name,
                SurName = e.Employee.SurName,
                PhoneNumber = e.Employee.PhoneNumber,
                Position = e.Employee.Position
            }).ToList()
        };
    }

    public async Task<PaginatedResponse<ReadRepairDto>> GetRepairsAsync(SieveModel sieveModel)
    {
        var query = _context.Repairs.AsQueryable();
        query = _sieveProcessor.Apply(sieveModel, query);
        var repairs = await query
            .Include(r => r.Car)
            .Include(r => r.RepairEmployees)
            .ThenInclude(re => re.Employee)
            .ToListAsync();

        var repairsDtos = repairs.Select(repair => new ReadRepairDto
        {
            RepairId = repair.Id,
            CreatedAt = repair.CreatedAt,
            FinishedAt = repair.FinishedAt,
            Car = repair.Car == null
                ? null
                : new ReadCarDto
                {
                    CarId = repair.Car.Id,
                    Brand = repair.Car.Brand,
                    Model = repair.Car.Model,
                    RegistrationNumber = repair.Car.RegistrationNumber,
                    Vin = repair.Car.Vin,
                    Year = repair.Car.Year
                },
            Description = repair.Description,
            Status = repair.Status,
            Price = repair.Price,
            Employees = repair.RepairEmployees.Select(e => new ReadEmployeeDto
            {
                EmployeeId = e.EmployeeId,
                Name = e.Employee.Name,
                SurName = e.Employee.SurName,
                PhoneNumber = e.Employee.PhoneNumber,
                Position = e.Employee.Position
            }).ToList()
        });
        return new PaginatedResponse<ReadRepairDto>
        {
            Data = repairsDtos,
            CurrentPage = sieveModel.Page ?? 1,
            PageSize = sieveModel.PageSize ?? 10,
            TotalCount = await query.CountAsync()
        };
    }

    public async Task UpdateRepairAsync(UpdateRepairDto updateRepairDto)
    {
        var repair = await _context.Repairs
            .Include(r => r.RepairEmployees)
            .FirstOrDefaultAsync(r => r.Id == updateRepairDto.RepairId);

        if (repair == null) throw new Exception("Repair not found");

        repair.Description = updateRepairDto.Description;
        repair.Status = updateRepairDto.Status;


        await _context.SaveChangesAsync();
    }

    public async Task UpdateRepairStatusAsync(Guid repairId, RepairStatusEnum status)
    {
        var repair = await _context.Repairs.FindAsync(repairId);

        if (repair == null) throw new Exception("Repair not found");

        if (status == RepairStatusEnum.Finished || status == RepairStatusEnum.Cancelled)
            if (repair.FinishedAt == null)
                repair.FinishedAt = DateTime.UtcNow;

        repair.Status = status;
        await _context.SaveChangesAsync();
    }

    public async Task CancelRepairAsync(Guid repairId)
    {
        var repair = await _context.Repairs.FindAsync(repairId);
        if (repair == null)
            throw new Exception("Repair not found");

        repair.Status = RepairStatusEnum.Cancelled;
        repair.FinishedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    public async Task FinishRepairAsync(Guid repairId, decimal finalPrice)
    {
        var repair = await _context.Repairs.FindAsync(repairId);
        if (repair == null)
            throw new Exception("Repair not found");

        repair.Status = RepairStatusEnum.Finished;
        repair.FinishedAt = DateTime.UtcNow;
        repair.Price = finalPrice;

        await _context.SaveChangesAsync();
    }
}