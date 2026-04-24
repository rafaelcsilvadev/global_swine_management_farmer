namespace GlobalSwineManagementFarmer.Api.src.Consumption.DTOs;

using FluentValidation;

public class ConsumptionDtoValidator : AbstractValidator<ConsumptionDto>
{
    public ConsumptionDtoValidator()
    {
        RuleFor(x => x.BagPetFood)
            .GreaterThanOrEqualTo(0).WithMessage("Feed consumption cannot be negative");

        RuleFor(x => x.WaterTank)
            .GreaterThanOrEqualTo(0).WithMessage("Water consumption cannot be negative");

        RuleFor(x => x.BatchId)
            .NotEmpty().WithMessage("Batch is required");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse is required");
    }
}
