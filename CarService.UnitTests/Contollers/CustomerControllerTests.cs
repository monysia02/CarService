using CarService.Controllers;
using CarService.CustomerService;
using CarService.DTOs.CustomerDto;
using CarService.Model;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sieve.Models;

namespace CarService.Tests.Controllers;

public class CustomerControllerTests
{
    private readonly CustomerController _controller;
    private readonly Mock<IValidator<CreateCustomerDto>> _createCustomerValidatorMock;
    private readonly Mock<ICustomerService> _customerServiceMock;
    private readonly Mock<IValidator<UpdateCustomerDto>> _updateCustomerValidatorMock;

    public CustomerControllerTests()
    {
        _createCustomerValidatorMock = new Mock<IValidator<CreateCustomerDto>>();
        _updateCustomerValidatorMock = new Mock<IValidator<UpdateCustomerDto>>();
        _customerServiceMock = new Mock<ICustomerService>();

        _controller = new CustomerController(
            _customerServiceMock.Object,
            _createCustomerValidatorMock.Object,
            _updateCustomerValidatorMock.Object);
    }

    [Fact]
    public async Task AddCustomerAsync_ShouldReturnOk_WhenValidationSucceeds()
    {
        var dto = new CreateCustomerDto
        {
            Name = "Test",
            SurName = "User",
            PhoneNumber = "123456789"
        };

        _createCustomerValidatorMock
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        var result = await _controller.AddCustomerAsync(dto);

        Assert.IsType<OkResult>(result);
        _customerServiceMock.Verify(s => s.AddCustomerAsync(dto), Times.Once);
    }

    [Fact]
    public async Task AddCustomerAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var dto = new CreateCustomerDto
        {
            Name = "Test",
            SurName = "User",
            PhoneNumber = "123456789"
        };

        var failure = new ValidationFailure("Name", "Required");
        var validationResult = new ValidationResult(new[] { failure });

        _createCustomerValidatorMock
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(validationResult);

        var result = await _controller.AddCustomerAsync(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<List<ValidationFailure>>(badRequest.Value);
        _customerServiceMock.Verify(s => s.AddCustomerAsync(It.IsAny<CreateCustomerDto>()), Times.Never);
    }

    [Fact]
    public async Task GetCustomerAsync_ShouldReturnOkWithCustomer()
    {
        var id = Guid.NewGuid();
        var dto = new ReadCustomerDto
        {
            CustomerId = id,
            Name = "John",
            SurName = "Doe",
            PhoneNumber = "999999999"
        };

        _customerServiceMock.Setup(s => s.GetCustomerAsync(id)).ReturnsAsync(dto);

        var result = await _controller.GetCustomerAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<ReadCustomerDto>(ok.Value);
        Assert.Equal(id, returned.CustomerId);
    }

    [Fact]
    public async Task GetCustomersAsync_ShouldReturnOkWithList()
    {
        var sieveModel = new SieveModel { Page = 1, PageSize = 10 };

        var paginated = new PaginatedResponse<ReadCustomerDto>
        {
            Data = new List<ReadCustomerDto>(),
            CurrentPage = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _customerServiceMock.Setup(s => s.GetCustomersAsync(sieveModel)).ReturnsAsync(paginated);

        var result = await _controller.GetCustomersAsync(sieveModel);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<PaginatedResponse<ReadCustomerDto>>(ok.Value);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldReturnOk_WhenValidationSucceeds()
    {
        var dto = new UpdateCustomerDto
        {
            CustomerId = Guid.NewGuid(),
            Name = "Updated",
            SurName = "Name",
            PhoneNumber = "000111222"
        };

        _updateCustomerValidatorMock
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        var result = await _controller.UpdateCustomerAsync(dto);

        Assert.IsType<OkResult>(result);
        _customerServiceMock.Verify(s => s.UpdateCustomerAsync(dto), Times.Once);
    }

    [Fact]
    public async Task UpdateCustomerAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var dto = new UpdateCustomerDto
        {
            CustomerId = Guid.NewGuid(),
            Name = "Updated",
            SurName = "Name",
            PhoneNumber = "000111222"
        };

        var failure = new ValidationFailure("PhoneNumber", "Invalid format");
        var validationResult = new ValidationResult(new[] { failure });

        _updateCustomerValidatorMock
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(validationResult);

        var result = await _controller.UpdateCustomerAsync(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<List<ValidationFailure>>(badRequest.Value);
        _customerServiceMock.Verify(s => s.UpdateCustomerAsync(It.IsAny<UpdateCustomerDto>()), Times.Never);
    }
}