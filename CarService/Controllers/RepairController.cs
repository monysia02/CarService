using CarService.DTOs.RepairDto;
using CarService.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

[ApiController]
[Route("api/[controller]")]
public class RepairController : ControllerBase
{
    private readonly IValidator<CreateRepairDto> _createRepairValidator;
    private readonly IRepairService _repairService;
    private readonly IValidator<UpdateRepairDto> _updateRepairValidator;

    public RepairController(
        IRepairService repairService,
        IValidator<CreateRepairDto> createRepairValidator,
        IValidator<UpdateRepairDto> updateRepairValidator)
    {
        _createRepairValidator = createRepairValidator;
        _updateRepairValidator = updateRepairValidator;
        _repairService = repairService;
    }

    [HttpPost]
    public async Task<IActionResult> AddRepairAsync(CreateRepairDto createRepairDto)
    {
        var validationResult = await _createRepairValidator.ValidateAsync(createRepairDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        await _repairService.AddRepairAsync(createRepairDto);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRepairAsync(Guid id)
    {
        var repair = await _repairService.GetRepairAsync(id);
        return Ok(repair);
    }

    [HttpGet]
    public async Task<IActionResult> GetRepairsAsync([FromQuery] SieveModel sieveModel)
    {
        var repairs = await _repairService.GetRepairsAsync(sieveModel);
        return Ok(repairs);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateRepairAsync([FromBody] UpdateRepairDto updateRepairDto)
    {
        var validationResult = await _updateRepairValidator.ValidateAsync(updateRepairDto);
        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        await _repairService.UpdateRepairAsync(updateRepairDto);
        return Ok();
    }

    [HttpPatch("cancelRepair")]
    public async Task<IActionResult> CancelRepairAsync([FromQuery] Guid repairId)
    {
        await _repairService.CancelRepairAsync(repairId);
        return Ok();
    }

    [HttpPatch("finishRepair")]
    public async Task<IActionResult> FinishRepairAsync([FromQuery] Guid repairId, [FromQuery] decimal finalPrice)
    {
        await _repairService.FinishRepairAsync(repairId, finalPrice);
        return Ok();
    }
}