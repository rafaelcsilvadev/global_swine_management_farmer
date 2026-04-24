namespace GlobalSwineManagementFarmer.Api.src.HealthMedication.DTOs;

using FluentValidation;

public class HealthMedicationDtoValidator : AbstractValidator<HealthMedicationDto>
{
    public HealthMedicationDtoValidator()
    {
        RuleFor(x => x.BatchId)
            .NotEmpty().WithMessage("Batch ID is required");

        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("Warehouse ID is required");

        RuleFor(x => x.TreatmentEfficacy)
            .NotEmpty().WithMessage("Treatment efficacy is required")
            .MaximumLength(500).WithMessage("Treatment efficacy cannot exceed 500 characters");

        RuleFor(x => x.Media)
            .MaximumLength(2000).WithMessage("Media cannot exceed 2000 characters")
            .When(x => x.Media != null);

        RuleFor(x => x.SymptomIds)
            .NotEmpty().WithMessage("At least one symptom must be provided")
            .Must(x => x != null && x.Count > 0).WithMessage("At least one symptom must be provided");
    }
}
