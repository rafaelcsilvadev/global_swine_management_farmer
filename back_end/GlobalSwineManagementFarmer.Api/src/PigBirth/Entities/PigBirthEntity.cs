namespace GlobalSwineManagementFarmer.Api.src.PigBirth.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class PigBirthEntity : BaseEntity
{
    [Required]
    public int LivesBirth { get; set; }

    [Required]
    public int StillBorn { get; set; }

    [Required]
    public int MummifiedBirth { get; set; }

    [Required]
    public Guid BatchId { get; set; }

    [Required]
    public Guid WarehouseId { get; set; }
}
