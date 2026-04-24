namespace GlobalSwineManagementFarmer.Api.src.Symptom.Entities;

using GlobalSwineManagementFarmer.Api.src.HealthMedication.Entities;
using System.ComponentModel.DataAnnotations;

public class SymptomObservedEntity
{
    [Required]
    public Guid SymptomId { get; set; }
    public SymptomEntity Symptom { get; set; } = null!;

    [Required]
    public Guid HealthMedicationId { get; set; }
    public HealthMedicationEntity HealthMedication { get; set; } = null!;
}
