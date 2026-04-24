namespace GlobalSwineManagementFarmer.Api.src.Consumption.DTOs;

public record ConsumptionDto(
    decimal BagPetFood,
    decimal WaterTank,
    Guid BatchId,
    Guid WarehouseId
);
