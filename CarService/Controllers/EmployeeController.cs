using CarService.DTOs.EmployeeDto;
using CarService.Services.EmployeeService;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IValidator<CreateEmployeeDto> _createEmployeeValidator;
    private readonly IEmployeeService _EmployeeService;
    private readonly IValidator<UpdateEmployeeDto> _updateEmployeeValidator;

    public EmployeeController(IEmployeeService EmployeeService, IValidator<CreateEmployeeDto> createEmployeeValidator,
        IValidator<UpdateEmployeeDto> updateEmployeeValidator)
    {
        _EmployeeService = EmployeeService;
        _createEmployeeValidator = createEmployeeValidator;
        _updateEmployeeValidator = updateEmployeeValidator;
    }

    [HttpPost]
    public async Task<IActionResult> AddEmployeeAsync(CreateEmployeeDto createEmployeeDto)
    {
        var validationResult = await _createEmployeeValidator.ValidateAsync(createEmployeeDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        await _EmployeeService.AddEmployeeAsync(createEmployeeDto);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEmployeeAsync(Guid id)
    {
        var Employee = await _EmployeeService.GetEmployeeAsync(id);
        return Ok(Employee);
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeesAsync([FromQuery] SieveModel sieveModel)
    {
        var Employees = await _EmployeeService.GetEmployeesAsync(sieveModel);
        return Ok(Employees);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeAsync(UpdateEmployeeDto updateEmployeeDto)
    {
        var validationResult = await _updateEmployeeValidator.ValidateAsync(updateEmployeeDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        await _EmployeeService.UpdateEmployeeAsync(updateEmployeeDto);
        return Ok();
    }
}