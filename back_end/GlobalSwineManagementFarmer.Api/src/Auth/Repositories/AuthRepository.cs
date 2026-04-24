namespace GlobalSwineManagementFarmer.Api.src.Auth.Repositories;

using BCrypt.Net;
using GlobalSwineManagementFarmer.Api.Data;
using GlobalSwineManagementFarmer.Api.src.Auth.DTOs;
using GlobalSwineManagementFarmer.Api.src.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthRepository(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    public async Task<AuthResponse> SignInAsync(SignInDto payload)
    {
        var farmer = await _db.Farmers
            .Include(f => f.Rule)
            .FirstOrDefaultAsync(f => f.Email == payload.Email)
            ?? throw new UnauthorizedAccessException("Email ou senha inválidos");

        if (!BCrypt.Verify(payload.Password, farmer.PasswordHash))
            throw new UnauthorizedAccessException("Email ou senha inválidos");

        var token = GenerateJwtToken(farmer.Id, farmer.Rule.Tag, farmer.Rule.Id);
        return new AuthResponse(token, int.Parse(_config["Jwt:ExpirationMinutes"]!));
    }

    public async Task CreateAsync(AuthDto payload)
    {
        if (await _db.Farmers.AnyAsync(f => f.Email == payload.Email))
            throw new InvalidOperationException("Email já cadastrado");

        var rule = await _db.Rules.FindAsync(payload.RuleId)
            ?? throw new InvalidOperationException("Role inválida");

        var farmer = new FarmerEntity
        {
            Email = payload.Email,
            PasswordHash = BCrypt.HashPassword(payload.Password),
            RuleId = rule.Id
        };

        _db.Farmers.Add(farmer);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, AuthDto payload)
    {
        var farmer = await _db.Farmers.FindAsync(id)
            ?? throw new KeyNotFoundException("Farmer não encontrado");

        farmer.Email = payload.Email;
        farmer.PasswordHash = BCrypt.HashPassword(payload.Password);
        farmer.RuleId = payload.RuleId;
        farmer.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var farmer = await _db.Farmers.FindAsync(id)
            ?? throw new KeyNotFoundException("Farmer não encontrado");

        farmer.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    private string GenerateJwtToken(Guid userId, string role, Guid ruleId)
    {
        var secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is missing");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] 
        { 
            new Claim("id", userId.ToString()),
            new Claim("ruleId", ruleId.ToString()),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpirationMinutes"]!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
