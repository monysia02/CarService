using CarService.DTOs.EmployeeDto;
using CarService.Services.EmployeeService;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarService.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _EmployeeService;

        public EmployeeController(IEmployeeService EmployeeService)
        {
            _EmployeeService = EmployeeService;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync(CreateEmployeeDto createEmployeeDto)
        {
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
            await _EmployeeService.UpdateEmployeeAsync(updateEmployeeDto);
            return Ok();
        }
    }
