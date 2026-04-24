namespace GlobalSwineManagementFarmer.Api.src.Batch.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class BatchEntity : BaseEntity
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public int DaysLife { get; set; }

    [Required]
    public Guid WarehouseId { get; set; }
}
