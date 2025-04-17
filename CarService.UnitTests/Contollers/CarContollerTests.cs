using CarService.CarService;
using CarService.Controllers;
using CarService.DTOs.CarDto;
using CarService.DTOs.CustomerDto;
using CarService.Model;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Sieve.Models;

namespace CarService.Tests.Controllers;

public class CarControllerTests
{
    private readonly Mock<ICarService> _carServiceMock;
    private readonly CarController _controller;
    private readonly Mock<IValidator<CreateCarDto>> _createCarValidatorMock;
    private readonly Mock<IValidator<UpdateCarDto>> _updateCarValidatorMock;

    public CarControllerTests()
    {
        _carServiceMock = new Mock<ICarService>();
        _createCarValidatorMock = new Mock<IValidator<CreateCarDto>>();
        _updateCarValidatorMock = new Mock<IValidator<UpdateCarDto>>();

        _controller = new CarController(
            _carServiceMock.Object,
            _createCarValidatorMock.Object,
            _updateCarValidatorMock.Object
        );
    }

    [Fact]
    public async Task AddCarAsync_ShouldReturnOk_WhenValidationPasses()
    {
        var dto = new CreateCarDto
        {
            Brand = "BMW",
            Model = "E46",
            Year = 2003,
            Vin = "VIN123456",
            RegistrationNumber = "KR1234",
            CustomerIds = new List<Guid>()
        };

        _createCarValidatorMock
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        var result = await _controller.AddCarAsync(dto);

        Assert.IsType<OkResult>(result);
        _carServiceMock.Verify(s => s.AddCarAsync(dto), Times.Once);
    }

    [Fact]
    public async Task AddCarAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var dto = new CreateCarDto
        {
            Brand = "Audi",
            Model = "A4",
            Year = 2015,
            Vin = "INVALID",
            RegistrationNumber = "WA1234",
            CustomerIds = new List<Guid>()
        };

        var failure = new ValidationFailure("Vin", "VIN is invalid");
        var validationResult = new ValidationResult(new[] { failure });

        _createCarValidatorMock
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(validationResult);

        var result = await _controller.AddCarAsync(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<List<ValidationFailure>>(badRequest.Value);
        _carServiceMock.Verify(s => s.AddCarAsync(It.IsAny<CreateCarDto>()), Times.Never);
    }

    [Fact]
    public async Task GetCarAsync_ShouldReturnOkWithCar()
    {
        var id = Guid.NewGuid();
        var readDto = new ReadCarDto
        {
            CarId = id,
            Brand = "Mazda",
            Model = "6",
            Year = 2019,
            Vin = "VIN000123",
            RegistrationNumber = "GDA1234",
            Customers = new List<ReadCustomerDto>()
        };

        _carServiceMock
            .Setup(s => s.GetCarAsync(id))
            .ReturnsAsync(readDto);

        var result = await _controller.GetCarAsync(id);

        var ok = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<ReadCarDto>(ok.Value);
        Assert.Equal(id, returned.CarId);
    }

    [Fact]
    public async Task GetCarsAsync_ShouldReturnOkWithPaginatedList()
    {
        var sieveModel = new SieveModel { Page = 1, PageSize = 10 };

        var paginated = new PaginatedResponse<ReadCarDto>
        {
            Data = new List<ReadCarDto>(),
            CurrentPage = 1,
            PageSize = 10,
            TotalCount = 0
        };

        _carServiceMock
            .Setup(s => s.GetCarsAsync(sieveModel))
            .ReturnsAsync(paginated);

        var result = await _controller.GetCarsAsync(sieveModel);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.IsType<PaginatedResponse<ReadCarDto>>(ok.Value);
    }

    [Fact]
    public async Task UpdateCarAsync_ShouldReturnOk_WhenValidationPasses()
    {
        var dto = new UpdateCarDto
        {
            CarId = Guid.NewGuid(),
            Brand = "Opel",
            Model = "Astra",
            Year = 2021,
            Vin = "OP123",
            RegistrationNumber = "PO12345"
        };

        _updateCarValidatorMock
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        var result = await _controller.UpdateCarAsync(dto);

        Assert.IsType<OkResult>(result);
        _carServiceMock.Verify(s => s.UpdateCarAsync(dto), Times.Once);
    }

    [Fact]
    public async Task UpdateCarAsync_ShouldReturnBadRequest_WhenValidationFails()
    {
        var dto = new UpdateCarDto
        {
            CarId = Guid.NewGuid(),
            Brand = "Fiat",
            Model = "Panda",
            Year = 2018,
            Vin = "BADVIN",
            RegistrationNumber = "FI1234"
        };

        var failure = new ValidationFailure("Brand", "Brand is required");
        var validationResult = new ValidationResult(new[] { failure });

        _updateCarValidatorMock
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(validationResult);

        var result = await _controller.UpdateCarAsync(dto);

        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<List<ValidationFailure>>(badRequest.Value);
        _carServiceMock.Verify(s => s.UpdateCarAsync(It.IsAny<UpdateCarDto>()), Times.Never);
    }
}