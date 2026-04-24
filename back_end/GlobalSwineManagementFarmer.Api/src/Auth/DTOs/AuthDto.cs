namespace GlobalSwineManagementFarmer.Api.src.Auth.DTOs;

public record AuthDto(string Email, string Password, Guid RuleId);
