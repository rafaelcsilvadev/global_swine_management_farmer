namespace GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;

using FluentValidation;

public class SymptomDtoValidator : AbstractValidator<SymptomDto>
{
    public SymptomDtoValidator()
    {
        RuleFor(x => x.Symptom)
            .NotEmpty().WithMessage("Symptom is required")
            .MaximumLength(100).WithMessage("Symptom cannot exceed 100 characters");
    }
}
