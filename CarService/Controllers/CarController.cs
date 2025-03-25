using CarService.CarService;
using CarService.DTOs.CarDto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly ICarService _CarService;
    private readonly IValidator<CreateCarDto> _createCarValidator;
    private readonly IValidator<UpdateCarDto> _updateCarValidator;

    public CarController(ICarService CarService, IValidator<CreateCarDto> createCarValidator,
        IValidator<UpdateCarDto> updateCarValidator)
    {
        _CarService = CarService;
        _createCarValidator = createCarValidator;
        _updateCarValidator = updateCarValidator;
    }

    [HttpPost]
    public async Task<IActionResult> AddCarAsync(CreateCarDto createCarDto)
    {
        var validationResult = await _createCarValidator.ValidateAsync(createCarDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        await _CarService.AddCarAsync(createCarDto);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCarAsync(Guid id)
    {
        var car = await _CarService.GetCarAsync(id);
        return Ok(car);
    }

    [HttpGet]
    public async Task<IActionResult> GetCarsAsync([FromQuery] SieveModel sieveModel)
    {
        var cars = await _CarService.GetCarsAsync(sieveModel);
        return Ok(cars);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCarAsync(UpdateCarDto updateCarDto)
    {
        var validationResult = await _updateCarValidator.ValidateAsync(updateCarDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        await _CarService.UpdateCarAsync(updateCarDto);
        return Ok();
    }
}