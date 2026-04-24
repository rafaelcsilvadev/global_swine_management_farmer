namespace GlobalSwineManagementFarmer.Api.Common.Middleware;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration  _config;

    public JwtMiddleware(RequestDelegate next, IConfiguration config)
    {
        _next   = next;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"]
                           .FirstOrDefault()?.Split(" ").Last();

        if (token is not null)
            AttachUserToContext(context, token);

        await _next(context);
    }

    private void AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret is missing");
            var key     = Encoding.UTF8.GetBytes(secret);
            var handler = new JwtSecurityTokenHandler();

            handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey         = new SymmetricSecurityKey(key),
                ValidateIssuer           = false,
                ValidateAudience         = false,
                ClockSkew                = TimeSpan.Zero
            }, out var validatedToken);

            var jwt    = (JwtSecurityToken)validatedToken;
            var userId = jwt.Claims.First(c => c.Type == "id").Value;
            var ruleId = jwt.Claims.First(c => c.Type == "ruleId").Value;
            var role   = jwt.Claims.First(c => c.Type == "role" || c.Type == ClaimTypes.Role).Value;

            context.Items["UserId"] = userId;
            context.Items["RuleId"] = ruleId;
            context.Items["UserRole"] = role;
        }
        catch
        {
            // Token inválido — prossegue sem UserId. [Authorize] barra nos endpoints protegidos.
        }
    }
}
