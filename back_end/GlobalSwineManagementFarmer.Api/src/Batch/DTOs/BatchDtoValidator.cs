namespace GlobalSwineManagementFarmer.Api.src.Batch.DTOs;

using FluentValidation;

public class BatchDtoValidator : AbstractValidator<BatchDto>
{
    public BatchDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters");

        RuleFor(x => x.DaysLife)
            .GreaterThanOrEqualTo(0).WithMessage("Days of life cannot be negative");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required");
    }
}
