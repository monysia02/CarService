using CarService.DTOs.RepairDto;
using CarService.Enums;
using CarService.Model;
using Sieve.Models;

namespace CarService.Services;

public interface IRepairService
{
    Task AddRepairAsync(CreateRepairDto repair);
    Task<ReadRepairDto> GetRepairAsync(Guid id);
    Task<PaginatedResponse<ReadRepairDto>> GetRepairsAsync(SieveModel sieveModel);
    Task UpdateRepairAsync(UpdateRepairDto repair);
    Task UpdateRepairStatusAsync(Guid repairId, RepairStatusEnum status);
    Task CancelRepairAsync(Guid repairId);
    Task FinishRepairAsync(Guid repairId, decimal finalPrice);
}