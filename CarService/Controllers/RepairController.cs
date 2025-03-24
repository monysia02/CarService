using CarService.DTOs.RepairDto;
using CarService.Enums;
using CarService.Services;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarService.Controllers;

[ApiController]
[Route("api/[controller]")]

public class RepairController : ControllerBase
{
    private readonly IRepairService _RepairService;

    public RepairController(IRepairService RepairService)
    {
        _RepairService = RepairService;
    }

    [HttpPost]
    public async Task<IActionResult> AddRepairAsync(CreateRepairDto createRepairDto)
    {
        await _RepairService.AddRepairAsync(createRepairDto);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRepairAsync(Guid id)
    {
        var Repair = await _RepairService.GetRepairAsync(id);
        return Ok(Repair);
    }

    [HttpGet]
    public async Task<IActionResult> GetRepairsAsync([FromQuery] SieveModel sieveModel)
    {
        var Repairs = await _RepairService.GetRepairsAsync(sieveModel);
        return Ok(Repairs);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRepairAsync(UpdateRepairDto updateRepairDto)
    {
        await _RepairService.UpdateRepairAsync(updateRepairDto);
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateRepairStatusAsync(Guid repairId, RepairStatusEnum status)
    {
        await _RepairService.UpdateRepairStatusAsync(repairId, status);
        return Ok();
    }
}