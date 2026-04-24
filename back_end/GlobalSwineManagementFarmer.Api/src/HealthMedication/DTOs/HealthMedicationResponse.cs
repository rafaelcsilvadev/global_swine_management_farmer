namespace GlobalSwineManagementFarmer.Api.src.HealthMedication.DTOs;

using GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;

public record HealthMedicationResponse(
    Guid Id,
    Guid BatchId,
    Guid WarehouseId,
    string? Media,
    string TreatmentEfficacy,
    List<SymptomResponse> Symptoms,
    DateTime CreatedAt
);
