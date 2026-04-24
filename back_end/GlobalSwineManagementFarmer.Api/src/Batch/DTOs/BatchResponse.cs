namespace GlobalSwineManagementFarmer.Api.src.Batch.DTOs;

public record BatchResponse(Guid Id, string Code, int DaysLife, Guid WarehouseId);
