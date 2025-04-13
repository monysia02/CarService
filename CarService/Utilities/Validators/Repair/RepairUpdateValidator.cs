using CarService.DTOs.RepairDto;
using FluentValidation;

namespace CarService.Utilities.Validators.Repair;

public class RepairUpdateValidator : AbstractValidator<UpdateRepairDto>
{
    public RepairUpdateValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(500)
            .WithMessage("Description must be less than 500 characters");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Status must be a valid RepairStatusEnum value");
    }
}