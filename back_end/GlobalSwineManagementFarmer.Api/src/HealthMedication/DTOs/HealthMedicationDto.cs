namespace GlobalSwineManagementFarmer.Api.src.HealthMedication.DTOs;

public record HealthMedicationDto(
    Guid BatchId,
    Guid WarehouseId,
    string? Media,
    string TreatmentEfficacy,
    List<Guid> SymptomIds
);
