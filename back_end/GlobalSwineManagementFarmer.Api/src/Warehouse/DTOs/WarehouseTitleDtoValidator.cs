namespace GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;

using FluentValidation;

public class WarehouseTitleDtoValidator : AbstractValidator<WarehouseTitleDto>
{
    public WarehouseTitleDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title cannot exceed 100 characters");
    }
}
