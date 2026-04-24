namespace GlobalSwineManagementFarmer.Api.src.Auth.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class FarmerEntity : BaseEntity
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public Guid RuleId { get; set; }
    public RuleEntity Rule { get; set; } = null!;
}
