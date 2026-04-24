namespace GlobalSwineManagementFarmer.Api.src.HealthMedication.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using GlobalSwineManagementFarmer.Api.src.Batch.Entities;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Entities;
using GlobalSwineManagementFarmer.Api.src.Symptom.Entities;
using System.ComponentModel.DataAnnotations;

public class HealthMedicationEntity : BaseEntity
{
    public string? Media { get; set; }

    [Required]
    public string TreatmentEfficacy { get; set; } = string.Empty;

    [Required]
    public Guid BatchId { get; set; }
    public BatchEntity Batch { get; set; } = null!;

    [Required]
    public Guid WarehouseId { get; set; }
    public WarehouseEntity Warehouse { get; set; } = null!;

    public ICollection<SymptomObservedEntity> SymptomsObserved { get; set; } = new List<SymptomObservedEntity>();
}
