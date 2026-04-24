namespace GlobalSwineManagementFarmer.Api.src.Batch.DTOs;

using FluentValidation;

public class BatchDtoValidator : AbstractValidator<BatchDto>
{
    public BatchDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Código é obrigatório")
            .MaximumLength(50).WithMessage("Código não pode ter mais de 50 caracteres");

        RuleFor(x => x.DaysLife)
            .GreaterThanOrEqualTo(0).WithMessage("Dias de vida não pode ser negativo");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("ID do galpão é obrigatório");
    }
}
