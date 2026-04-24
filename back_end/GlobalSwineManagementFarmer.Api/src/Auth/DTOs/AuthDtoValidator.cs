namespace GlobalSwineManagementFarmer.Api.src.Auth.DTOs;

using FluentValidation;

public class AuthDtoValidator : AbstractValidator<AuthDto>
{
    public AuthDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email inválido")
            .MaximumLength(20).WithMessage("Email não pode ter mais de 20 caracteres")
            .Must(e => !e.Contains("'") && !e.Contains("--") && !e.Contains(";"))
            .WithMessage("Email contém caracteres não permitidos");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres")
            .MaximumLength(15).WithMessage("Senha não pode ter mais de 15 caracteres")
            .Matches(@"[A-Z]").WithMessage("Senha deve conter pelo menos uma letra maiúscula")
            .Matches(@"[a-z]").WithMessage("Senha deve conter pelo menos uma letra minúscula")
            .Matches(@"[0-9]").WithMessage("Senha deve conter pelo menos um número")
            .Matches(@"[\W_]").WithMessage("Senha deve conter pelo menos um caractere especial");

        RuleFor(x => x.RuleId)
            .NotEmpty().WithMessage("Role ID é obrigatório")
            .Must(id => id != Guid.Empty).WithMessage("Role ID inválido");
    }
}
