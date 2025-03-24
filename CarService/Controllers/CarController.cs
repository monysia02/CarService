using CarService.CarService;
using CarService.DTOs.CarDto;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarService.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CarController : ControllerBase
{
    private readonly ICarService _CarService;

    public CarController(ICarService CarService)
    {
        _CarService = CarService;
    }

    [HttpPost]
    public async Task<IActionResult> AddCarAsync(CreateCarDto createCarDto)
    {
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
        await _CarService.UpdateCarAsync(updateCarDto);
        return Ok();
    }
}
