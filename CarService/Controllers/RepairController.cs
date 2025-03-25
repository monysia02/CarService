using CarService.DTOs.RepairDto;
using CarService.Enums;
using CarService.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepairController : ControllerBase
{
    private readonly IValidator<CreateRepairDto> _createRepairValidator;
    private readonly IRepairService _RepairService;
    private readonly IValidator<UpdateRepairDto> _updateRepairValidator;

    public RepairController(IRepairService RepairService)
    {
        _RepairService = RepairService;
    }

    [HttpPost]
    public async Task<IActionResult> AddRepairAsync(CreateRepairDto createRepairDto)
    {
        var validationResult = await _createRepairValidator.ValidateAsync(createRepairDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

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
        var validationResult = await _updateRepairValidator.ValidateAsync(updateRepairDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

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