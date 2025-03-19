using CarService.DTOs.RepairDto;
using CarService.Services;
using Microsoft.AspNetCore.Mvc;

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
        var Repair = await _RepairService.GetRepairsAsync();
        return Ok(Repair);
    }

    [HttpGet]
    public async Task<IActionResult> GetRepairsAsync()
    {
        var Repairs = await _RepairService.GetRepairsAsync();
        return Ok(Repairs);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRepairAsync(UpdateRepairDto updateRepairDto)
    {
        await _RepairService.UpdateRepairAsync(updateRepairDto);
        return Ok();
    }
}