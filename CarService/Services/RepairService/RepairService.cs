using CarService.Data;
using CarService.DTOs.CarDto;    
using CarService.DTOs.EmployeeDto;
using CarService.DTOs.RepairDto;
using CarService.Enums;
using CarService.Model;
using Microsoft.EntityFrameworkCore;

namespace CarService.Services
{
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

            if (repair == null)
            {
                throw new Exception("Repair not found");
            }

            return new ReadRepairDto
            {
                RepairId = repair.Id,
                CreatedAt = repair.CreatedAt,
                FinishedAt = repair.FinishedAt,
                CarId = repair.CarId,
                Car = repair.Car == null ? null : new ReadCarDto
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

        public async Task<IEnumerable<ReadRepairDto>> GetRepairsAsync()
        {
            var repairs = await _context.Repairs
                .Include(r => r.Car)  // Dołączamy powiązany samochód
                .Include(r => r.RepairEmployees)
                    .ThenInclude(re => re.Employee)
                .ToListAsync();

            return repairs.Select(repair => new ReadRepairDto
            {
                RepairId = repair.Id,
                CreatedAt = repair.CreatedAt,
                FinishedAt = repair.FinishedAt,
                CarId = repair.CarId,
                Car = repair.Car == null ? null : new ReadCarDto
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
            repair.FinishedAt = updateRepairDto.FinishedAt.HasValue
                ? updateRepairDto.FinishedAt.Value.ToUniversalTime()
                : null;

            repair.RepairEmployees = updateRepairDto.EmployeeIds
                .Select(employeeId => new RepairEmployee
                {
                    RepairId = repair.Id,
                    EmployeeId = employeeId
                }).ToList();

            await _context.SaveChangesAsync();
        }
    }
}