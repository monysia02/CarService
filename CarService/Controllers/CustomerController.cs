using CarService.CustomerService;
using CarService.DTOs.CustomerDto;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly IValidator<CreateCustomerDto> _createCustomerValidator;
    private readonly ICustomerService _customerService;
    private readonly IValidator<UpdateCustomerDto> _updateCustomerValidator;

    public CustomerController(ICustomerService customerService, IValidator<CreateCustomerDto> createCustomerValidator,
        IValidator<UpdateCustomerDto> updateCustomerValidator)
    {
        _customerService = customerService;
        _createCustomerValidator = createCustomerValidator;
        _updateCustomerValidator = updateCustomerValidator;
    }

    [HttpPost]
    public async Task<IActionResult> AddCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        var validationResult = await _createCustomerValidator.ValidateAsync(createCustomerDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);
        await _customerService.AddCustomerAsync(createCustomerDto);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerAsync(Guid id)
    {
        var customer = await _customerService.GetCustomerAsync(id);
        return Ok(customer);
    }

    [HttpGet]
    public async Task<IActionResult> GetCustomersAsync([FromQuery] SieveModel sieveModel)
    {
        var customers = await _customerService.GetCustomersAsync(sieveModel);
        return Ok(customers);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCustomerAsync(UpdateCustomerDto updateCustomerDto)
    {
        var validationResult = await _updateCustomerValidator.ValidateAsync(updateCustomerDto);
        if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

        await _customerService.UpdateCustomerAsync(updateCustomerDto);
        return Ok();
    }
}