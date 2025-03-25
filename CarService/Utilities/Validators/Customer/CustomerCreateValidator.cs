using CarService.DTOs.CustomerDto;
using FluentValidation;

namespace CarService.Utilities.Validators.Customer;

public class CustomerCreateValidator : AbstractValidator<CreateCustomerDto>
{
    public CustomerCreateValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(25).WithMessage(
                "Name must be less than 25 characters"
            );

        RuleFor(x => x.SurName)
            .NotEmpty()
            .MaximumLength(50)
            .WithMessage(
                "Surname must be less than 50 characters"
            );

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MaximumLength(50)
            .Matches(@"^\d{9}$")
            .WithMessage(
                "Phone number must be 9 digits"
            );
    }
}