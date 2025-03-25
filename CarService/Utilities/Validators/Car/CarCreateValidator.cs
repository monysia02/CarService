using CarService.DTOs.CarDto;
using FluentValidation;

namespace CarService.Utilities.Validators.Car;

public class CarCreateValidator : AbstractValidator<CreateCarDto>
{
    public CarCreateValidator()
    {
        RuleFor(x => x.CustomerIds)
            .NotEmpty()
            .WithMessage("CustomerIds must not be empty");

        RuleFor(x => x.Brand)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Brand must be less than 50 characters");

        RuleFor(x => x.Model)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage("Model must be less than 50 characters");


        RuleFor(x => x.Vin)
            .NotEmpty()
            .MaximumLength(17)
            .WithMessage("Vin must be less than 17 characters");

        RuleFor(x => x.RegistrationNumber)
            .NotEmpty()
            .MaximumLength(10)
            .WithMessage("Registration number must be less than 10 characters");
    }
}