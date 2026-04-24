namespace GlobalSwineManagementFarmer.Api.src.Consumption.DTOs;

public record ConsumptionResponse(
    Guid Id,
    decimal BagPetFood,
    decimal WaterTank,
    Guid BatchId,
    Guid WarehouseId
);
