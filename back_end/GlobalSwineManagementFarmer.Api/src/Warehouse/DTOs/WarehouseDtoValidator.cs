namespace GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;

using FluentValidation;

public class WarehouseDtoValidator : AbstractValidator<WarehouseDto>
{
    public WarehouseDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Código é obrigatório")
            .MaximumLength(50).WithMessage("Código não pode ter mais de 50 caracteres");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Título é obrigatório")
            .MaximumLength(100).WithMessage("Título não pode ter mais de 100 caracteres");
    }
}
