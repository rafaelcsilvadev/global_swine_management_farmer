namespace GlobalSwineManagementFarmer.Api.src.Auth.DTOs;

public record AuthResponse(string Token, int ExpirationMinutes);
