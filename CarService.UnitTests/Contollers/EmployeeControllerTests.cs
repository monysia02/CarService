using CarService.Controllers;
using CarService.DTOs.EmployeeDto;
using CarService.Model;
using CarService.Services.EmployeeService;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sieve.Models;

namespace CarService.Tests.Contollers;

public class EmployeeControllerTests
{
    private readonly EmployeeController _controller;
    private readonly Mock<IValidator<CreateEmployeeDto>> _createEmployeeValidatorMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly Mock<IValidator<UpdateEmployeeDto>> _updateEmployeeValidatorMock;

    public EmployeeControllerTests()
    {
        _employeeServiceMock = new Mock<IEmployeeService>();
        _createEmployeeValidatorMock = new Mock<IValidator<CreateEmployeeDto>>();
        _updateEmployeeValidatorMock = new Mock<IValidator<UpdateEmployeeDto>>();

        _controller = new EmployeeController(
            _employeeServiceMock.Object,
            _createEmployeeValidatorMock.Object,
            _updateEmployeeValidatorMock.Object);
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnOk_WhenValidationSucceeds()
    {
        var createEmployeeDto = new CreateEmployeeDto
        {
            Name = "Test",
            SurName = "User",
            PhoneNumber = "123456789",
            Position = "Mechanik"
        };

        _createEmployeeValidatorMock
            .Setup(v => v.ValidateAsync(createEmployeeDto, default))
            .ReturnsAsync(new ValidationResult());

        var result = await _controller.AddEmployeeAsync(createEmployeeDto);

        Assert.IsType<OkResult>(result);
        _employeeServiceMock.Verify(s => s.AddEmployeeAsync(createEmployeeDto), Times.Once);
    }

    [Fact]
    public async Task AddEmployeeAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var createEmployeeDto = new CreateEmployeeDto
        {
            Name = "Test",
            SurName = "User",
            PhoneNumber = "123456789",
            Position = "Mechanik"
        };

        var failure = new ValidationFailure("Name", "Name is required");
        var validationResult = new ValidationResult(new[] { failure });

        _createEmployeeValidatorMock
            .Setup(v => v.ValidateAsync(createEmployeeDto, default))
            .ReturnsAsync(validationResult);

        var result = await _controller.AddEmployeeAsync(createEmployeeDto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
        _employeeServiceMock.Verify(s => s.AddEmployeeAsync(It.IsAny<CreateEmployeeDto>()), Times.Never);
    }

    [Fact]
    public async Task GetEmployeeAsync_ShouldReturnOkWithEmployee_WhenEmployeeExists()
    {
        var employeeId = Guid.NewGuid();
        var readEmployeeDto = new ReadEmployeeDto
        {
            EmployeeId = employeeId,
            Name = "Test",
            SurName = "User",
            PhoneNumber = "123456789",
            Position = "Mechanik"
        };

        _employeeServiceMock
            .Setup(s => s.GetEmployeeAsync(employeeId))
            .ReturnsAsync(readEmployeeDto);

        var result = await _controller.GetEmployeeAsync(employeeId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedDto = Assert.IsType<ReadEmployeeDto>(okResult.Value);
        Assert.Equal(employeeId, returnedDto.EmployeeId);
    }

    [Fact]
    public async Task GetEmployeesAsync_ShouldReturnOkWithEmployeeList()
    {
        var sieveModel = new SieveModel
        {
            Page = 1,
            PageSize = 10
        };

        var paginatedResponse = new PaginatedResponse<ReadEmployeeDto>
        {
            Data = new List<ReadEmployeeDto>(),
            CurrentPage = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _employeeServiceMock
            .Setup(s => s.GetEmployeesAsync(sieveModel))
            .ReturnsAsync(paginatedResponse);

        var result = await _controller.GetEmployeesAsync(sieveModel);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<PaginatedResponse<ReadEmployeeDto>>(okResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnOk_WhenValidationSucceeds()
    {
        var updateEmployeeDto = new UpdateEmployeeDto
        {
            EmployeeId = Guid.NewGuid(),
            Name = "Updated",
            SurName = "User",
            PhoneNumber = "987654321",
            Position = "Starszy Mechanik"
        };

        _updateEmployeeValidatorMock
            .Setup(v => v.ValidateAsync(updateEmployeeDto, default))
            .ReturnsAsync(new ValidationResult());

        var result = await _controller.UpdateEmployeeAsync(updateEmployeeDto);

        Assert.IsType<OkResult>(result);
        _employeeServiceMock.Verify(s => s.UpdateEmployeeAsync(updateEmployeeDto), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var updateEmployeeDto = new UpdateEmployeeDto
        {
            EmployeeId = Guid.NewGuid(),
            Name = "Updated",
            SurName = "User",
            PhoneNumber = "987654321",
            Position = "Starszy Mechanik"
        };

        var failure = new ValidationFailure("Name", "Name cannot be empty");
        var validationResult = new ValidationResult(new[] { failure });

        _updateEmployeeValidatorMock
            .Setup(v => v.ValidateAsync(updateEmployeeDto, default))
            .ReturnsAsync(validationResult);

        var result = await _controller.UpdateEmployeeAsync(updateEmployeeDto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
        _employeeServiceMock.Verify(s => s.UpdateEmployeeAsync(updateEmployeeDto), Times.Never);
    }
}