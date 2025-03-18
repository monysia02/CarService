using CarService.Data;
using CarService.DTOs.RepairDto;
using CarService.Enums;
using CarService.Model;
using Microsoft.EntityFrameworkCore;

namespace CarService.Services.RepairService;

public class RepairService : IRepairService
{
    private readonly ApplicationDbContext _context;

    public RepairService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRepairAsync(CreateRepairDto repairDto)
    {
        var createdRepairGuid = Guid.NewGuid();

        var repair = new Repair
        {
            Id = createdRepairGuid,
            CreatedAt = DateTime.Now,
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
        var repair = await _context.Repairs.FindAsync(id); //??
        return new ReadRepairDto(); //??
    }

    public async Task<IEnumerable<ReadRepairDto>> GetRepairsAsync()
    {
        var repairs = await _context.Repairs
            .Include(e => e.RepairEmployees)
            .ToListAsync();
        return repairs.Select(e => new ReadRepairDto()); //???
    }

    public async Task UpdateRepairAsync(UpdateRepairDto updateRepairDto)
    {
        var repair = await _context.Repairs
            .Include(r => r.RepairEmployees)
            .FirstOrDefaultAsync(r => r.Id == updateRepairDto.RepairId);

        if (repair == null)
        {
            throw new Exception("Repair not found");
        }

        repair.Description = updateRepairDto.Description;
        repair.Status = updateRepairDto.Status;
        repair.FinishedAt = updateRepairDto.FinishedAt;

        repair.RepairEmployees = updateRepairDto.EmployeeIds
            .Select(employeeId => new RepairEmployee
            {
                RepairId = repair.Id,
                EmployeeId = employeeId
            }).ToList();

        await _context.SaveChangesAsync();
    }
}