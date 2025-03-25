using CarService.DTOs.RepairDto;
using FluentValidation;

namespace CarService.Utilities.Validators.Repair;

public class RepairUpdateValidator : AbstractValidator<UpdateRepairDto>
{
    public RepairUpdateValidator()
    {
        RuleFor(x => x.CarId)
            .NotEmpty()
            .WithMessage("CarId is required");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Description must be less than 500 characters");

        RuleFor(x => x.EmployeeIds)
            .NotEmpty()
            .WithMessage("EmployeeIds is required");

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}