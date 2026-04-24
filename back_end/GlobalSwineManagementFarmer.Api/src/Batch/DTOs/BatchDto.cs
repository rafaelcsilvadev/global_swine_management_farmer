namespace GlobalSwineManagementFarmer.Api.src.Batch.DTOs;

public record BatchDto(string Code, int DaysLife, Guid WarehouseId);
