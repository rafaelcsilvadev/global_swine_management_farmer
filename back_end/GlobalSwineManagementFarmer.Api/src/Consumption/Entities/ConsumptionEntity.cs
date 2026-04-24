namespace GlobalSwineManagementFarmer.Api.src.Consumption.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using GlobalSwineManagementFarmer.Api.src.Batch.Entities;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Entities;
using System.ComponentModel.DataAnnotations.Schema;

public class ConsumptionEntity : BaseEntity
{
    public decimal BagPetFood { get; set; }
    public decimal WaterTank { get; set; }

    public Guid BatchId { get; set; }
    [ForeignKey("BatchId")]
    public BatchEntity Batch { get; set; } = null!;

    public Guid WarehouseId { get; set; }
    [ForeignKey("WarehouseId")]
    public WarehouseEntity Warehouse { get; set; } = null!;
}
