namespace GlobalSwineManagementFarmer.Api.src.Auth.Entities;

using GlobalSwineManagementFarmer.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class RuleEntity : BaseEntity
{
    [Required]
    public string Tag { get; set; } = string.Empty;
}
