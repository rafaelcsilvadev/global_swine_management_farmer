namespace GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;

public record PigBirthResponse(Guid Id, int LivesBirth, int StillBorn, int MummifiedBirth, Guid BatchId, Guid WarehouseId);
