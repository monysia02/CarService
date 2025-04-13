using CarService.DTOs.RepairDto;
using CarService.Services;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarService.UnitTests.Controllers;

public class RepairControllerTests
{
    private readonly RepairController _controller;
    private readonly Mock<IValidator<CreateRepairDto>> _createRepairValidatorMock;
    private readonly Mock<IRepairService> _repairServiceMock;
    private readonly Mock<IValidator<UpdateRepairDto>> _updateRepairValidatorMock;

    public RepairControllerTests()
    {
        _repairServiceMock = new Mock<IRepairService>();
        _createRepairValidatorMock = new Mock<IValidator<CreateRepairDto>>();
        _updateRepairValidatorMock = new Mock<IValidator<UpdateRepairDto>>();

        _controller = new RepairController(
            _repairServiceMock.Object,
            _createRepairValidatorMock.Object,
            _updateRepairValidatorMock.Object);
    }

    [Fact]
    public async Task AddRepairAsync_ShouldReturnOk_WhenValidationSucceeds()
    {
        var createRepairDto = new CreateRepairDto
        {
            CarId = Guid.NewGuid(),
            Description = "Test repair",
            EmployeeIds = new List<Guid> { Guid.NewGuid() }
        };

        _createRepairValidatorMock
            .Setup(v => v.ValidateAsync(createRepairDto, default))
            .ReturnsAsync(new ValidationResult());

        var result = await _controller.AddRepairAsync(createRepairDto);

        Assert.IsType<OkResult>(result);
        _repairServiceMock.Verify(s => s.AddRepairAsync(createRepairDto), Times.Once);
    }

    [Fact]
    public async Task AddRepairAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var createRepairDto = new CreateRepairDto
        {
            CarId = Guid.NewGuid(),
            Description = "Test repair",
            EmployeeIds = new List<Guid> { Guid.NewGuid() }
        };

        var failure = new ValidationFailure("CarId", "CarId is invalid");
        var validationResult = new ValidationResult(new[] { failure });

        _createRepairValidatorMock
            .Setup(v => v.ValidateAsync(createRepairDto, default))
            .ReturnsAsync(validationResult);

        var result = await _controller.AddRepairAsync(createRepairDto);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
        _repairServiceMock.Verify(s => s.AddRepairAsync(It.IsAny<CreateRepairDto>()), Times.Never);
    }
}