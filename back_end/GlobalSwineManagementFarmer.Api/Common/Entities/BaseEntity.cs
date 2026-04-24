namespace GlobalSwineManagementFarmer.Api.Common.Entities;

using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; }

    public DateTime  CreatedAt { get; set; }
    public DateTime  UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
