using CarService.CustomerService;
using CarService.Model;
using Microsoft.AspNetCore.Mvc;

namespace CarService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomerAsync(Customer customer)
        {
            await _customerService.AddCustomerAsync(customer);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAsync(Guid id)
        {
            var customer = await _customerService.GetCustomerAsync(id);
            return Ok(customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersAsync()
        {
            var customers = await _customerService.GetCustomersAsync();
            return Ok(customers);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomerAsync(Customer customer)
        {
            await _customerService.UpdateCustomerAsync(customer);
            return Ok();
        }
    }
}