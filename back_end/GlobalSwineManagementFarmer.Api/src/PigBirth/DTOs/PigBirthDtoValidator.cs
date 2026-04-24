namespace GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;

using FluentValidation;

public class PigBirthDtoValidator : AbstractValidator<PigBirthDto>
{
    public PigBirthDtoValidator()
    {
        RuleFor(x => x.LivesBirth)
            .GreaterThanOrEqualTo(0).WithMessage("Live births cannot be negative");

        RuleFor(x => x.StillBorn)
            .GreaterThanOrEqualTo(0).WithMessage("Stillborns cannot be negative");

        RuleFor(x => x.MummifiedBirth)
            .GreaterThanOrEqualTo(0).WithMessage("Mummified births cannot be negative");

        RuleFor(x => x.BatchId)
            .NotEmpty().WithMessage("Batch ID is required");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required");
    }
}
