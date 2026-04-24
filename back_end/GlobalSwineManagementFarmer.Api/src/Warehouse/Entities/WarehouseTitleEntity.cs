namespace GlobalSwineManagementFarmer.Api.src.Warehouse.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class WarehouseTitleEntity : BaseEntity
{
    [Required]
    public string Title { get; set; } = string.Empty;
}
