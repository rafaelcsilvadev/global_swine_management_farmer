namespace GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;

using FluentValidation;

public class WarehouseDtoValidator : AbstractValidator<WarehouseDto>
{
    public WarehouseDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
    }
}
