using CarService.DTOs.RepairDto;

namespace CarService.Services.RepairService;

public interface IRepairService
{
    Task AddRepairAsync(CreateRepairDto repair);
    Task<ReadRepairDto> GetRepairAsync(Guid id);
    Task<IEnumerable<ReadRepairDto>> GetRepairsAsync();
    Task UpdateRepairAsync(UpdateRepairDto repair);
}