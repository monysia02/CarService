using CarService.DTOs.RepairDto;
using FluentValidation;

namespace CarService.Utilities.Validators.Repair;

public class RepairCreateValidator : AbstractValidator<CreateRepairDto>
{
    public RepairCreateValidator()
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
    }
}