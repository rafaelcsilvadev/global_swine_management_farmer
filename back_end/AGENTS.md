# AGENTS.md — Global Swine Management Farmer · Módulo Tratador

> Documento de referência para agentes de IA e desenvolvedores.
> Stack: **.NET 8 Web API** · EF Core · PostgreSQL · Docker · Nginx
> Padrão: **Controller → Repository → Entity** por módulo isolado
> **UMA CLASSE POR ARQUIVO SEMPRE**

---

## 1. Visão Geral

O módulo **Tratador** é responsável pelo registro operacional diário em uma granja suína. Após autenticação JWT, o tratador seleciona o galpão/lote/grupo e pode registrar:

- Partos (nascidos vivos, natimortos, mumificados)
- Consumo de ração e água
- Saúde e medicação (sintomas, imagens, eficácia do tratamento)

---

## 2. Fluxo de Negócio (Business Rules)

```
Actor
  └─► Faz login com e-mail/senha
        └─► Selecionar galpão / lote / grupo
              └─► Registrar partos
                    └─► Registrar nascidos vivos / natimortos / mumificados
                          └─► Registrar consumo de insumos
                                └─► Registrar consumo de ração e água
                                      └─► Registrar saúde
                                            └─► Registrar sintomas, imagens e eficácia do tratamento
```

---

## 3. Modelo de Dados (Database)

Todas as tabelas herdam os campos de auditoria: `created_at`, `updated_at`, `deleted_at`.

| Entidade             | PK   | Campos principais                      | FK(s)                         |
|----------------------|------|----------------------------------------|-------------------------------|
| **Farmer**           | id   | email (UNIQUE), password (hash BCrypt) | ruleId → Rule                 |
| **Rule**             | id   | tag (UNIQUE)                           | —                             |
| **Warehouse**        | id   | code (UNIQUE)                          | titleId → WarehouseTitle      |
| **WarehouseTitle**   | id   | title (UNIQUE)                         | —                             |
| **Batch**            | id   | code (UNIQUE), daysLife                | warehouseId → Warehouse       |
| **Pig**              | id   | litterNumber, pigNumber, birthDate     | pigStatusId, batchId          |
| **PigStatus**        | id   | status (UNIQUE)                        | —                             |
| **PigBirth**         | id   | livesBirth, stillBorn, mummifiedBirth  | batchId, warehouseId          |
| **Consumption**      | id   | bagPetFood, waterTank                  | batchId, warehouseId          |
| **HealthMedication** | id   | media, treatmentEfficacy               | batchId, warehouseId          |
| **SymptomObserved**  | —    | (tabela de junção N:N)                 | symptomId, healthMedicationId |
| **Symptom**          | id   | symptom (UNIQUE)                       | —                             |

---

## 4. Estrutura de Projetos e Pastas

```
GlobalSwineManagement/
│
├── GlobalSwineManagement.Api/               ← Projeto principal Web API
│   ├── GlobalSwineManagement.Api.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   │
│   ├── Data/
│   │   ├── AppDbContext.cs                  ← DbContext central (EF Core)
│   │   └── Migrations/                     ← Geradas pelo EF CLI
│   │
│   ├── Common/
│   │   ├── Entities/
│   │   │   └── BaseEntity.cs               ← Id, CreatedAt, UpdatedAt, DeletedAt
│   │   └── Middleware/
│   │       └── JwtMiddleware.cs            ← Validação customizada do token JWT
│   │
│   └── src/                                 ← Todos os módulos de negócio ficam aqui
│       ├── Auth/
│       │   ├── Controllers/
│       │   │   └── AuthController.cs
│       │   ├── Repositories/
│       │   │   ├── IAuthRepository.cs
│       │   │   └── AuthRepository.cs
│       │   ├── Entities/
│       │   │   ├── FarmerEntity.cs
│       │   │   └── RuleEntity.cs
│       │   └── DTOs/
│       │       ├── AuthDto.cs
│       │       ├── AuthDtoValidator.cs      ← FluentValidation
│       │       ├── AuthResponse.cs
│       │       └── AuthResponseValidator.cs
│       │
│       ├── Batch/
│       │   ├── Controllers/
│       │   │   └── BatchController.cs
│       │   ├── Repositories/
│       │   │   ├── IBatchRepository.cs
│       │   │   └── BatchRepository.cs
│       │   ├── Entities/
│       │   │   └── BatchEntity.cs
│       │   └── DTOs/
│       │       ├── BatchDto.cs
│       │       └── BatchDtoValidator.cs
│       │
│       ├── Warehouse/
│       │   ├── Controllers/
│       │   │   └── WarehouseController.cs
│       │   ├── Repositories/
│       │   │   ├── IWarehouseRepository.cs
│       │   │   └── WarehouseRepository.cs
│       │   ├── Entities/
│       │   │   ├── WarehouseEntity.cs
│       │   │   └── WarehouseTitleEntity.cs
│       │   └── DTOs/
│       │       ├── WarehouseDto.cs
│       │       └── WarehouseDtoValidator.cs
│       │
│       ├── Pig/
│       │   ├── Controllers/
│       │   │   └── PigController.cs
│       │   ├── Repositories/
│       │   │   ├── IPigRepository.cs
│       │   │   └── PigRepository.cs
│       │   ├── Entities/
│       │   │   ├── PigEntity.cs
│       │   │   └── PigStatusEntity.cs
│       │   └── DTOs/
│       │       ├── PigDto.cs
│       │       └── PigDtoValidator.cs
│       │
│       ├── PigBirth/
│       │   ├── Controllers/
│       │   │   └── PigBirthController.cs
│       │   ├── Repositories/
│       │   │   ├── IPigBirthRepository.cs
│       │   │   └── PigBirthRepository.cs
│       │   ├── Entities/
│       │   │   └── PigBirthEntity.cs
│       │   └── DTOs/
│       │       ├── PigBirthDto.cs
│       │       └── PigBirthDtoValidator.cs
│       │
│       ├── Consumption/
│       │   ├── Controllers/
│       │   │   └── ConsumptionController.cs
│       │   ├── Repositories/
│       │   │   ├── IConsumptionRepository.cs
│       │   │   └── ConsumptionRepository.cs
│       │   ├── Entities/
│       │   │   └── ConsumptionEntity.cs
│       │   └── DTOs/
│       │       ├── ConsumptionDto.cs
│       │       └── ConsumptionDtoValidator.cs
│       │
│       ├── HealthMedication/
│       │   ├── Controllers/
│       │   │   └── HealthMedicationController.cs
│       │   ├── Repositories/
│       │   │   ├── IHealthMedicationRepository.cs
│       │   │   └── HealthMedicationRepository.cs
│       │   ├── Entities/
│       │   │   └── HealthMedicationEntity.cs
│       │   └── DTOs/
│       │       ├── HealthMedicationDto.cs
│       │       └── HealthMedicationDtoValidator.cs
│       │
│       └── Symptom/
│           ├── Controllers/
│           │   └── SymptomController.cs
│           ├── Repositories/
│           │   ├── ISymptomRepository.cs
│           │   └── SymptomRepository.cs
│           ├── Entities/
│           │   ├── SymptomEntity.cs
│           │   └── SymptomObservedEntity.cs
│           └── DTOs/
│               ├── SymptomDto.cs
│               └── SymptomDtoValidator.cs
│
├── GlobalSwineManagement.Tests/             ← Projeto xUnit isolado
│   ├── GlobalSwineManagement.Tests.csproj
│   ├── Auth/
│   │   ├── AuthRepositoryTests.cs
│   │   └── AuthControllerTests.cs           ← Testes de validação
│   ├── Batch/
│   │   ├── BatchRepositoryTests.cs
│   │   └── BatchControllerTests.cs
│   ├── Warehouse/
│   │   ├── WarehouseRepositoryTests.cs
│   │   └── WarehouseControllerTests.cs
│   ├── Pig/
│   │   ├── PigRepositoryTests.cs
│   │   └── PigControllerTests.cs
│   ├── PigBirth/
│   │   ├── PigBirthRepositoryTests.cs
│   │   └── PigBirthControllerTests.cs
│   ├── Consumption/
│   │   ├── ConsumptionRepositoryTests.cs
│   │   └── ConsumptionControllerTests.cs
│   ├── HealthMedication/
│   │   ├── HealthMedicationRepositoryTests.cs
│   │   └── HealthMedicationControllerTests.cs
│   └── Symptom/
│       ├── SymptomRepositoryTests.cs
│       └── SymptomControllerTests.cs
│
├── docker-compose.yml
├── Dockerfile
├── nginx/
│   └── nginx.conf
└── GlobalSwineManagement.sln
```

---

## 5. BaseEntity — Auditoria em Todas as Tabelas

```csharp
// Common/Entities/BaseEntity.cs
namespace GlobalSwineManagement.Api.Common.Entities;

using System.ComponentModel.DataAnnotations;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime  CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime  UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; }   // null = ativo; preenchido = soft delete
}
```

---

## 6. Segurança — Auth .NET Core + JWT

### 6.1 Pacotes necessários

```xml
<!-- GlobalSwineManagement.Api.csproj -->
<ItemGroup>
  <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer"  Version="8.*" />
  <PackageReference Include="System.IdentityModel.Tokens.Jwt"                Version="7.*" />
  <PackageReference Include="BCrypt.Net-Next"                                Version="4.*" />
  <PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL"          Version="8.*" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.Design"           Version="8.*" />
  <PackageReference Include="FluentValidation.AspNetCore"                    Version="11.*" />
</ItemGroup>
```

### 6.2 Configuração no Program.cs

```csharp
// Program.cs
using FluentValidation;
using FluentValidation.AspNetCore;
using GlobalSwineManagement.Api.Common.Middleware;
using GlobalSwineManagement.Api.Data;
using GlobalSwineManagement.Api.src.Auth.Repositories;
using GlobalSwineManagement.Api.src.Batch.Repositories;
using GlobalSwineManagement.Api.src.Warehouse.Repositories;
using GlobalSwineManagement.Api.src.Pig.Repositories;
using GlobalSwineManagement.Api.src.PigBirth.Repositories;
using GlobalSwineManagement.Api.src.Consumption.Repositories;
using GlobalSwineManagement.Api.src.HealthMedication.Repositories;
using GlobalSwineManagement.Api.src.Symptom.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── JWT Authentication ──────────────────────────────────────────────────
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = builder.Configuration["Jwt:Issuer"],
            ValidAudience            = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey         = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });

builder.Services.AddAuthorization();

// ── Database ─────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// ── Repositories ─────────────────────────────────────────────────────────
builder.Services.AddScoped<IAuthRepository,            AuthRepository>();
builder.Services.AddScoped<IBatchRepository,           BatchRepository>();
builder.Services.AddScoped<IWarehouseRepository,       WarehouseRepository>();
builder.Services.AddScoped<IPigRepository,             PigRepository>();
builder.Services.AddScoped<IPigBirthRepository,        PigBirthRepository>();
builder.Services.AddScoped<IConsumptionRepository,     ConsumptionRepository>();
builder.Services.AddScoped<IHealthMedicationRepository, HealthMedicationRepository>();
builder.Services.AddScoped<ISymptomRepository,         SymptomRepository>();

// ── FluentValidation ─────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();  // Auto-registra todos os validators

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ── Migrations automáticas ───────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

// ── Middleware pipeline (ORDEM OBRIGATÓRIA) ──────────────────────────────
app.UseAuthentication();              // 1º — identifica quem é
app.UseAuthorization();               // 2º — verifica o que pode fazer
app.UseMiddleware<JwtMiddleware>();   // 3º — validações customizadas

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
```

### 6.3 appsettings.json

```json
{
  "Jwt": {
    "Secret":              "TROQUE_POR_UMA_CHAVE_DE_32_CARACTERES_MINIMO",
    "Issuer":              "GlobalSwineManagement",
    "Audience":            "TratadorApp",
    "ExpirationMinutes":   60
  },
  "ConnectionStrings": {
    "Default": "Host=postgres;Port=5432;Database=swine_db;Username=swine;Password=swine_pass"
  }
}
```

### 6.4 Middleware de Validação JWT

```csharp
// Common/Middleware/JwtMiddleware.cs
namespace GlobalSwineManagement.Api.Common.Middleware;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
            var key     = Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!);
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

            context.Items["UserId"] = userId;
        }
        catch
        {
            // Token inválido — prossegue sem UserId. [Authorize] barra nos endpoints protegidos.
        }
    }
}
```

---

## 7. Módulos — Uma Classe por Arquivo + FluentValidation

> **REGRA ABSOLUTA:** Uma classe por arquivo. Uma interface por arquivo. Um record por arquivo. Um validator por arquivo.

---

### 7.1 Módulo Auth

#### src/Auth/Entities/FarmerEntity.cs
```csharp
namespace GlobalSwineManagement.Api.src.Auth.Entities;

using GlobalSwineManagement.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class FarmerEntity : BaseEntity
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;  // UNIQUE

    [Required]
    public string PasswordHash { get; set; } = string.Empty;  // BCrypt hash — NUNCA senha em texto

    public Guid RuleId { get; set; }
    public RuleEntity Rule { get; set; } = null!;
}
```

#### src/Auth/Entities/RuleEntity.cs
```csharp
namespace GlobalSwineManagement.Api.src.Auth.Entities;

using GlobalSwineManagement.Api.Common.Entities;
using System.ComponentModel.DataAnnotations;

public class RuleEntity : BaseEntity
{
    [Required]
    public string Tag { get; set; } = string.Empty;  // UNIQUE — ex: "admin", "tratador"
}
```

#### src/Auth/DTOs/AuthDto.cs
```csharp
namespace GlobalSwineManagement.Api.src.Auth.DTOs;

public record AuthDto(string Email, string Password, string? Rule);
```

#### src/Auth/DTOs/AuthDtoValidator.cs
```csharp
namespace GlobalSwineManagement.Api.src.Auth.DTOs;

using FluentValidation;

public class AuthDtoValidator : AbstractValidator<AuthDto>
{
    public AuthDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email inválido")
            .MaximumLength(255).WithMessage("Email não pode ter mais de 255 caracteres");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres")
            .MaximumLength(100).WithMessage("Senha não pode ter mais de 100 caracteres")
            .Matches(@"[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula")
            .Matches(@"[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula")
            .Matches(@"[0-9]").WithMessage("Senha deve conter pelo menos um número")
            .Matches(@"[\W_]").WithMessage("Senha deve conter pelo menos um caractere especial");

        RuleFor(x => x.Rule)
            .MaximumLength(50).WithMessage("Role não pode ter mais de 50 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Rule));
    }
}
```

#### src/Auth/DTOs/AuthResponse.cs
```csharp
namespace GlobalSwineManagement.Api.src.Auth.DTOs;

public record AuthResponse(string Token, int ExpirationMinutes);
```

#### src/Auth/Repositories/IAuthRepository.cs
```csharp
namespace GlobalSwineManagement.Api.src.Auth.Repositories;

using GlobalSwineManagement.Api.src.Auth.DTOs;

public interface IAuthRepository
{
    Task<AuthResponse> SignInAsync(AuthDto payload);
    Task CreateAsync(AuthDto payload);
    Task UpdateAsync(Guid id, AuthDto payload);
    Task DeleteAsync(Guid id);
}
```

#### src/Auth/Repositories/AuthRepository.cs
```csharp
namespace GlobalSwineManagement.Api.src.Auth.Repositories;

using BCrypt.Net;
using GlobalSwineManagement.Api.Data;
using GlobalSwineManagement.Api.src.Auth.DTOs;
using GlobalSwineManagement.Api.src.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext   _db;
    private readonly IConfiguration _config;

    public AuthRepository(AppDbContext db, IConfiguration config)
    {
        _db     = db;
        _config = config;
    }

    public async Task<AuthResponse> SignInAsync(AuthDto payload)
    {
        var farmer = await _db.Farmers
            .FirstOrDefaultAsync(f => f.Email == payload.Email)
            ?? throw new UnauthorizedAccessException("Email ou senha inválidos");

        if (!BCrypt.Verify(payload.Password, farmer.PasswordHash))
            throw new UnauthorizedAccessException("Email ou senha inválidos");

        var token = GenerateJwtToken(farmer.Id);
        return new AuthResponse(token, int.Parse(_config["Jwt:ExpirationMinutes"]!));
    }

    public async Task CreateAsync(AuthDto payload)
    {
        if (await _db.Farmers.AnyAsync(f => f.Email == payload.Email))
            throw new InvalidOperationException("Email já cadastrado");

        var rule = payload.Rule is not null
            ? await _db.Rules.FirstOrDefaultAsync(r => r.Tag == payload.Rule)
              ?? throw new InvalidOperationException("Role inválida")
            : await _db.Rules.FirstAsync(r => r.Tag == "tratador");  // default

        var farmer = new FarmerEntity
        {
            Email        = payload.Email,
            PasswordHash = BCrypt.HashPassword(payload.Password),
            RuleId       = rule.Id
        };

        _db.Farmers.Add(farmer);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Guid id, AuthDto payload)
    {
        var farmer = await _db.Farmers.FindAsync(id)
            ?? throw new KeyNotFoundException("Farmer não encontrado");

        farmer.Email        = payload.Email;
        farmer.PasswordHash = BCrypt.HashPassword(payload.Password);
        farmer.UpdatedAt    = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var farmer = await _db.Farmers.FindAsync(id)
            ?? throw new KeyNotFoundException("Farmer não encontrado");

        farmer.DeletedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }

    private string GenerateJwtToken(Guid userId)
    {
        var key    = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
        var creds  = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[] { new Claim("id", userId.ToString()) };

        var token = new JwtSecurityToken(
            issuer:   _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:   claims,
            expires:  DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpirationMinutes"]!)),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

#### src/Auth/Controllers/AuthController.cs
```csharp
namespace GlobalSwineManagement.Api.src.Auth.Controllers;

using GlobalSwineManagement.Api.src.Auth.DTOs;
using GlobalSwineManagement.Api.src.Auth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _repo;

    public AuthController(IAuthRepository repo) => _repo = repo;

    [AllowAnonymous]
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] AuthDto dto)
    {
        try
        {
            return Ok(await _repo.SignInAsync(dto));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AuthDto dto)
    {
        try
        {
            await _repo.CreateAsync(dto);
            return Created();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] AuthDto dto)
    {
        try
        {
            await _repo.UpdateAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
```

---

### 7.2 Módulo Batch (exemplo resumido)

#### src/Batch/DTOs/BatchDto.cs
```csharp
namespace GlobalSwineManagement.Api.src.Batch.DTOs;

public record BatchDto(string Code, string DaysLife, Guid WarehouseId);
```

#### src/Batch/DTOs/BatchDtoValidator.cs
```csharp
namespace GlobalSwineManagement.Api.src.Batch.DTOs;

using FluentValidation;

public class BatchDtoValidator : AbstractValidator<BatchDto>
{
    public BatchDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Código do lote é obrigatório")
            .MaximumLength(50).WithMessage("Código não pode ter mais de 50 caracteres")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("Código deve conter apenas letras, números, hífens e underscores");

        RuleFor(x => x.DaysLife)
            .NotEmpty().WithMessage("Dias de vida é obrigatório")
            .MaximumLength(20).WithMessage("Dias de vida não pode ter mais de 20 caracteres");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("ID do galpão é obrigatório");
    }
}
```

---

### 7.3 Padrão de Validação para os demais módulos

Todos os DTOs seguem o mesmo padrão:

#### WarehouseDtoValidator
```csharp
public class WarehouseDtoValidator : AbstractValidator<WarehouseDto>
{
    public WarehouseDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Código do galpão é obrigatório")
            .MaximumLength(50).WithMessage("Código não pode ter mais de 50 caracteres")
            .Matches(@"^[a-zA-Z0-9\-_]+$").WithMessage("Código deve conter apenas letras, números, hífens e underscores");

        RuleFor(x => x.TitleId)
            .NotEmpty().WithMessage("ID do título é obrigatório");
    }
}
```

#### PigDtoValidator
```csharp
public class PigDtoValidator : AbstractValidator<PigDto>
{
    public PigDtoValidator()
    {
        RuleFor(x => x.LitterNumber)
            .GreaterThanOrEqualTo(0).WithMessage("Número da ninhada não pode ser negativo");

        RuleFor(x => x.PigNumber)
            .GreaterThan(0).WithMessage("Número do suíno deve ser maior que zero");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("Data de nascimento é obrigatória")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data de nascimento não pode ser futura");

        RuleFor(x => x.PigStatusId)
            .NotEmpty().WithMessage("ID do status é obrigatório");

        RuleFor(x => x.BatchId)
            .NotEmpty().WithMessage("ID do lote é obrigatório");
    }
}
```

#### PigBirthDtoValidator
```csharp
public class PigBirthDtoValidator : AbstractValidator<PigBirthDto>
{
    public PigBirthDtoValidator()
    {
        RuleFor(x => x.LivesBirth)
            .GreaterThanOrEqualTo(0).WithMessage("Nascidos vivos não pode ser negativo");

        RuleFor(x => x.StillBorn)
            .GreaterThanOrEqualTo(0).WithMessage("Natimortos não pode ser negativo");

        RuleFor(x => x.MummifiedBirth)
            .GreaterThanOrEqualTo(0).WithMessage("Mumificados não pode ser negativo");

        RuleFor(x => x.BatchId)
            .NotEmpty().WithMessage("ID do lote é obrigatório");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("ID do galpão é obrigatório");
    }
}
```

#### ConsumptionDtoValidator
```csharp
public class ConsumptionDtoValidator : AbstractValidator<ConsumptionDto>
{
    public ConsumptionDtoValidator()
    {
        RuleFor(x => x.BagPetFood)
            .GreaterThanOrEqualTo(0).WithMessage("Sacos de ração não pode ser negativo");

        RuleFor(x => x.WaterTank)
            .GreaterThanOrEqualTo(0).WithMessage("Tanques de água não pode ser negativo");

        RuleFor(x => x.BatchId)
            .NotEmpty().WithMessage("ID do lote é obrigatório");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("ID do galpão é obrigatório");
    }
}
```

#### HealthMedicationDtoValidator
```csharp
public class HealthMedicationDtoValidator : AbstractValidator<HealthMedicationDto>
{
    public HealthMedicationDtoValidator()
    {
        RuleFor(x => x.Media)
            .MaximumLength(500).WithMessage("URL/caminho da mídia não pode ter mais de 500 caracteres")
            .When(x => !string.IsNullOrEmpty(x.Media));

        RuleFor(x => x.TreatmentEfficacy)
            .NotEmpty().WithMessage("Eficácia do tratamento é obrigatória")
            .MaximumLength(100).WithMessage("Eficácia não pode ter mais de 100 caracteres");

        RuleFor(x => x.BatchId)
            .NotEmpty().WithMessage("ID do lote é obrigatório");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("ID do galpão é obrigatório");

        RuleFor(x => x.SymptomIds)
            .NotNull().WithMessage("Lista de sintomas não pode ser nula");
    }
}
```

#### SymptomDtoValidator
```csharp
public class SymptomDtoValidator : AbstractValidator<SymptomDto>
{
    public SymptomDtoValidator()
    {
        RuleFor(x => x.Symptom)
            .NotEmpty().WithMessage("Descrição do sintoma é obrigatória")
            .MinimumLength(3).WithMessage("Sintoma deve ter no mínimo 3 caracteres")
            .MaximumLength(200).WithMessage("Sintoma não pode ter mais de 200 caracteres");
    }
}
```

---

## 8. Testes de Validação — Controllers

### 8.1 Pacotes de teste

```xml
<!-- GlobalSwineManagement.Tests/GlobalSwineManagement.Tests.csproj -->
<ItemGroup>
  <PackageReference Include="xunit"                                  Version="2.*" />
  <PackageReference Include="xunit.runner.visualstudio"              Version="2.*" />
  <PackageReference Include="Microsoft.NET.Test.Sdk"                 Version="17.*" />
  <PackageReference Include="Moq"                                    Version="4.*" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.InMemory" Version="8.*" />
  <PackageReference Include="FluentAssertions"                       Version="6.*" />
  <PackageReference Include="Microsoft.AspNetCore.Mvc.Testing"       Version="8.*" />
</ItemGroup>
```

### 8.2 Exemplo — AuthControllerTests

```csharp
// GlobalSwineManagement.Tests/Auth/AuthControllerTests.cs
namespace GlobalSwineManagement.Tests.Auth;

using FluentAssertions;
using GlobalSwineManagement.Api.src.Auth.Controllers;
using GlobalSwineManagement.Api.src.Auth.DTOs;
using GlobalSwineManagement.Api.src.Auth.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class AuthControllerTests
{
    private readonly Mock<IAuthRepository> _mockRepo;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _mockRepo   = new Mock<IAuthRepository>();
        _controller = new AuthController(_mockRepo.Object);
    }

    // ── Testes de Validação de Entrada ─────────────────────────────────

    [Fact]
    public async Task Create_EmailVazio_DeveRetornarBadRequest()
    {
        var dto = new AuthDto("", "Senha@123", null);
        
        // Simula validação do FluentValidation
        _controller.ModelState.AddModelError("Email", "Email é obrigatório");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_EmailInvalido_DeveRetornarBadRequest()
    {
        var dto = new AuthDto("email_invalido", "Senha@123", null);

        _controller.ModelState.AddModelError("Email", "Email inválido");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_SenhaCurta_DeveRetornarBadRequest()
    {
        var dto = new AuthDto("user@swine.com", "123", null);

        _controller.ModelState.AddModelError("Password", "Senha deve ter no mínimo 8 caracteres");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_SenhaSemMaiuscula_DeveRetornarBadRequest()
    {
        var dto = new AuthDto("user@swine.com", "senha@123", null);

        _controller.ModelState.AddModelError("Password", "Senha deve conter pelo menos uma letra maiúscula");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_SenhaSemNumero_DeveRetornarBadRequest()
    {
        var dto = new AuthDto("user@swine.com", "Senha@abc", null);

        _controller.ModelState.AddModelError("Password", "Senha deve conter pelo menos um número");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_SenhaSemCaractereEspecial_DeveRetornarBadRequest()
    {
        var dto = new AuthDto("user@swine.com", "Senha1234", null);

        _controller.ModelState.AddModelError("Password", "Senha deve conter pelo menos um caractere especial");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_SQLInjectionNoEmail_DeveSerBloqueado()
    {
        var dto = new AuthDto("admin'--", "Senha@123", null);

        _controller.ModelState.AddModelError("Email", "Email inválido");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_XSSNoEmail_DeveSerBloqueado()
    {
        var dto = new AuthDto("<script>alert('XSS')</script>@test.com", "Senha@123", null);

        _controller.ModelState.AddModelError("Email", "Email inválido");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    // ── Testes de Sucesso ───────────────────────────────────────────────

    [Fact]
    public async Task SignIn_CredenciaisValidas_DeveRetornarToken()
    {
        var dto = new AuthDto("user@swine.com", "Senha@123", null);
        var response = new AuthResponse("fake_jwt_token", 60);

        _mockRepo.Setup(r => r.SignInAsync(dto)).ReturnsAsync(response);

        var result = await _controller.SignIn(dto);

        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.Value.Should().BeEquivalentTo(response);
    }

    [Fact]
    public async Task Create_DadosValidos_DeveRetornarCreated()
    {
        var dto = new AuthDto("newuser@swine.com", "Senha@123", null);

        _mockRepo.Setup(r => r.CreateAsync(dto)).Returns(Task.CompletedTask);

        var result = await _controller.Create(dto);

        result.Should().BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task SignIn_CredenciaisInvalidas_DeveRetornarUnauthorized()
    {
        var dto = new AuthDto("wrong@swine.com", "WrongPass@123", null);

        _mockRepo.Setup(r => r.SignInAsync(dto))
            .ThrowsAsync(new UnauthorizedAccessException("Email ou senha inválidos"));

        var result = await _controller.SignIn(dto);

        result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    [Fact]
    public async Task Create_EmailDuplicado_DeveRetornarConflict()
    {
        var dto = new AuthDto("duplicate@swine.com", "Senha@123", null);

        _mockRepo.Setup(r => r.CreateAsync(dto))
            .ThrowsAsync(new InvalidOperationException("Email já cadastrado"));

        var result = await _controller.Create(dto);

        result.Should().BeOfType<ConflictObjectResult>();
    }
}
```

### 8.3 Exemplo — BatchControllerTests

```csharp
// GlobalSwineManagement.Tests/Batch/BatchControllerTests.cs
namespace GlobalSwineManagement.Tests.Batch;

using FluentAssertions;
using GlobalSwineManagement.Api.src.Batch.Controllers;
using GlobalSwineManagement.Api.src.Batch.DTOs;
using GlobalSwineManagement.Api.src.Batch.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class BatchControllerTests
{
    private readonly Mock<IBatchRepository> _mockRepo;
    private readonly BatchController _controller;

    public BatchControllerTests()
    {
        _mockRepo   = new Mock<IBatchRepository>();
        _controller = new BatchController(_mockRepo.Object);
    }

    [Fact]
    public async Task Create_CodigoVazio_DeveRetornarBadRequest()
    {
        var dto = new BatchDto("", "30 dias", Guid.NewGuid());

        _controller.ModelState.AddModelError("Code", "Código do lote é obrigatório");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_CodigoComCaracteresEspeciais_DeveRetornarBadRequest()
    {
        var dto = new BatchDto("LOTE@2024#", "30 dias", Guid.NewGuid());

        _controller.ModelState.AddModelError("Code", "Código deve conter apenas letras, números, hífens e underscores");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_CodigoMuitoLongo_DeveRetornarBadRequest()
    {
        var dto = new BatchDto(new string('A', 51), "30 dias", Guid.NewGuid());

        _controller.ModelState.AddModelError("Code", "Código não pode ter mais de 50 caracteres");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_WarehouseIdVazio_DeveRetornarBadRequest()
    {
        var dto = new BatchDto("LOTE-001", "30 dias", Guid.Empty);

        _controller.ModelState.AddModelError("WarehouseId", "ID do galpão é obrigatório");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Create_SQLInjectionNoCodigo_DeveSerBloqueado()
    {
        var dto = new BatchDto("LOTE'; DROP TABLE Batches;--", "30 dias", Guid.NewGuid());

        _controller.ModelState.AddModelError("Code", "Código deve conter apenas letras, números, hífens e underscores");

        var result = await _controller.Create(dto);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
```

### 8.4 Padrão de testes para os demais módulos

Todos os controllers devem ter testes cobrindo:

1. **Validação de campos obrigatórios** — campos vazios/nulos
2. **Validação de formato** — emails inválidos, regex não atendida
3. **Validação de tamanho** — strings muito longas/curtas
4. **Validação de valores** — números negativos quando não permitido, datas futuras quando não permitido
5. **Proteção contra SQL Injection** — aspas simples, comandos SQL
6. **Proteção contra XSS** — scripts HTML/JS
7. **Proteção contra path traversal** — `../../../etc/passwd`
8. **Validação de GUIDs** — GUIDs vazios quando obrigatório
9. **Testes de sucesso** — dados válidos retornam status correto
10. **Testes de erro esperado** — duplicatas, não encontrado, etc.

---

## 9. AppDbContext (sem alterações)

```csharp
// Data/AppDbContext.cs
namespace GlobalSwineManagement.Api.Data;

using GlobalSwineManagement.Api.Common.Entities;
using GlobalSwineManagement.Api.src.Auth.Entities;
using GlobalSwineManagement.Api.src.Batch.Entities;
using GlobalSwineManagement.Api.src.Consumption.Entities;
using GlobalSwineManagement.Api.src.HealthMedication.Entities;
using GlobalSwineManagement.Api.src.Pig.Entities;
using GlobalSwineManagement.Api.src.PigBirth.Entities;
using GlobalSwineManagement.Api.src.Symptom.Entities;
using GlobalSwineManagement.Api.src.Warehouse.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<FarmerEntity>             Farmers            { get; set; }
    public DbSet<RuleEntity>               Rules              { get; set; }
    public DbSet<WarehouseEntity>          Warehouses         { get; set; }
    public DbSet<WarehouseTitleEntity>     WarehouseTitles    { get; set; }
    public DbSet<BatchEntity>              Batches            { get; set; }
    public DbSet<PigEntity>                Pigs               { get; set; }
    public DbSet<PigStatusEntity>          PigStatuses        { get; set; }
    public DbSet<PigBirthEntity>           PigBirths          { get; set; }
    public DbSet<ConsumptionEntity>        Consumptions       { get; set; }
    public DbSet<HealthMedicationEntity>   HealthMedications  { get; set; }
    public DbSet<SymptomEntity>            Symptoms           { get; set; }
    public DbSet<SymptomObservedEntity>    SymptomsObserved   { get; set; }

    protected override void OnModelCreating(ModelBuilder m)
    {
        // ── Unique Indexes ──────────────────────────────────────────────
        m.Entity<FarmerEntity>()          .HasIndex(e => e.Email)   .IsUnique();
        m.Entity<RuleEntity>()            .HasIndex(e => e.Tag)     .IsUnique();
        m.Entity<BatchEntity>()           .HasIndex(e => e.Code)    .IsUnique();
        m.Entity<WarehouseEntity>()       .HasIndex(e => e.Code)    .IsUnique();
        m.Entity<WarehouseTitleEntity>()  .HasIndex(e => e.Title)   .IsUnique();
        m.Entity<PigStatusEntity>()       .HasIndex(e => e.Status)  .IsUnique();
        m.Entity<SymptomEntity>()         .HasIndex(e => e.Symptom) .IsUnique();

        m.Entity<PigEntity>()
            .HasIndex(e => new { e.BatchId, e.PigNumber })
            .IsUnique();

        m.Entity<SymptomObservedEntity>()
            .HasKey(e => new { e.SymptomId, e.HealthMedicationId });

        // ── Soft delete global ──────────────────────────────────────────
        m.Entity<FarmerEntity>()          .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<RuleEntity>()            .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<BatchEntity>()           .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<WarehouseEntity>()       .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<WarehouseTitleEntity>()  .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<PigEntity>()             .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<PigStatusEntity>()       .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<PigBirthEntity>()        .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<ConsumptionEntity>()     .HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<HealthMedicationEntity>().HasQueryFilter(e => e.DeletedAt == null);
        m.Entity<SymptomEntity>()         .HasQueryFilter(e => e.DeletedAt == null);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(ct);
    }

    private void UpdateTimestamps()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>()
                     .Where(e => e.State == EntityState.Modified))
            entry.Entity.UpdatedAt = DateTime.UtcNow;
    }
}
```

---

## 10. Docker, Nginx, Migrations (sem alterações — ver versão anterior)

---

## 11. Resumo de Endpoints

| Módulo             | Método | Rota                          | Auth | Ação                |
|--------------------|--------|-------------------------------|------|---------------------|
| Auth               | POST   | `/api/auth/signin`            | ✗    | Login — retorna JWT |
| Auth               | POST   | `/api/auth`                   | ✗    | Criar farmer        |
| Auth               | PUT    | `/api/auth/{id}`              | ✓    | Atualizar farmer    |
| Auth               | DELETE | `/api/auth/{id}`              | ✓    | Soft delete farmer  |
| Batch              | POST   | `/api/batch`                  | ✓    | Criar lote          |
| Batch              | GET    | `/api/batch`                  | ✓    | Listar lotes        |
| Batch              | GET    | `/api/batch/{id}`             | ✓    | Buscar por ID       |
| Batch              | PUT    | `/api/batch/{id}`             | ✓    | Atualizar           |
| Batch              | DELETE | `/api/batch/{id}`             | ✓    | Soft delete         |
| *(demais módulos seguem mesmo padrão)*                                                |

---

## 12. Regras de Implementação para o Agente

1. **UMA CLASSE POR ARQUIVO** — nunca agrupar múltiplas classes/records/interfaces/validators no mesmo arquivo.
2. **TODO DTO TEM SEU VALIDATOR** — arquivo separado `XxxDtoValidator.cs` implementando `AbstractValidator<XxxDto>`.
3. **FluentValidation registrado globalmente** — `AddFluentValidationAutoValidation()` + `AddValidatorsFromAssemblyContaining<Program>()`.
4. **Toda entity herda `BaseEntity`** — nunca redeclarar `Id`, `CreatedAt`, `UpdatedAt`, `DeletedAt`.
5. **Senha sempre em hash BCrypt** — campo se chama `PasswordHash`, nunca `Password`.
6. **Soft delete obrigatório** — nunca `_db.Remove()`. Sempre `entity.DeletedAt = DateTime.UtcNow`.
7. **IDs são `Guid` gerados server-side** — nunca aceitar `Id` no body de POST.
8. **Email único globalmente** — verificar duplicidade; retornar `409 Conflict` se existir.
9. **Campos únicos** (Code, Tag, Status, Symptom, Title) — capturar `InvalidOperationException`; Controller retorna `409 Conflict`.
10. **Nunca lógica de negócio no Controller** — só recebe, delega ao Repository, retorna HTTP status.
11. **Validação protege contra SQL Injection e XSS** — usar regex adequada em validators (ex: `^[a-zA-Z0-9\-_]+$` para códigos).
12. **Testes de controller cobrem validação** — campos obrigatórios, tamanho, formato, SQL injection, XSS, path traversal.
13. **Testes de repository cobrem lógica** — duplicatas, soft delete, hash de senha, etc.
14. **Repositories sempre async** — usar `ToListAsync`, `FirstOrDefaultAsync`, `SaveChangesAsync`.
15. **Migrations rodam automaticamente** no startup via `db.Database.MigrateAsync()`.
16. **JWT Secret nunca em código-fonte** — sempre via variável de ambiente.
17. Corrigir typos: `Werehouse → Warehouse`, `Consuption → Consumption`, `Entitiy → Entity`, `mummifed → mummified`, `efficancy → efficacy`.
