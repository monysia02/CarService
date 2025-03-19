using CarService.DTOs.RepairDto;
using CarService.Enums;

namespace CarService.Services;
public interface IRepairService
{
    Task AddRepairAsync(CreateRepairDto repair);
    Task<ReadRepairDto> GetRepairAsync(Guid id);
    Task<IEnumerable<ReadRepairDto>> GetRepairsAsync();
    Task UpdateRepairAsync(UpdateRepairDto repair);
    Task UpdateRepairStatusAsync(Guid repairId, RepairStatusEnum status);
}