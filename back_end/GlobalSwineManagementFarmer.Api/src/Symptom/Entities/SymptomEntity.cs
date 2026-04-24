namespace GlobalSwineManagementFarmer.Api.src.Symptom.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class SymptomEntity : BaseEntity
{
    [Required]
    public string Symptom { get; set; } = string.Empty; // UNIQUE

    public ICollection<SymptomObservedEntity> SymptomsObserved { get; set; } = new List<SymptomObservedEntity>();
}
