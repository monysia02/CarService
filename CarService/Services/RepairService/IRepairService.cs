using CarService.DTOs.RepairDto;

namespace CarService.Services;
public interface IRepairService
{
    Task AddRepairAsync(CreateRepairDto repair);
    Task<ReadRepairDto> GetRepairAsync(Guid id);
    Task<IEnumerable<ReadRepairDto>> GetRepairsAsync();
    Task UpdateRepairAsync(UpdateRepairDto repair);
}