namespace GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;

public record PigBirthDto(int LivesBirth, int StillBorn, int MummifiedBirth, Guid BatchId, Guid WarehouseId);
