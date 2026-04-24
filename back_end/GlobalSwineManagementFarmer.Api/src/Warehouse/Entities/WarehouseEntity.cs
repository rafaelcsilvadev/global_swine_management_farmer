namespace GlobalSwineManagementFarmer.Api.src.Warehouse.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class WarehouseEntity : BaseEntity
{
    [Required]
    public string Code { get; set; } = string.Empty;

    public Guid TitleId { get; set; }
    public WarehouseTitleEntity Title { get; set; } = null!;
}
