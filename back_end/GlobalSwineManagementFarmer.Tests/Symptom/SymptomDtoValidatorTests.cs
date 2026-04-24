namespace GlobalSwineManagementFarmer.Tests.Symptom;

using GlobalSwineManagementFarmer.Api.src.Symptom.DTOs;
using Xunit;

public class SymptomDtoValidatorTests
{
    private readonly SymptomDtoValidator _validator;

    public SymptomDtoValidatorTests()
    {
        _validator = new SymptomDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Symptom_Is_Empty()
    {
        var dto = new SymptomDto("");
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Symptom");
    }

    [Fact]
    public void Should_Have_Error_When_Symptom_Is_Too_Long()
    {
        var dto = new SymptomDto(new string('a', 101));
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Symptom");
    }

    [Fact]
    public void Should_Be_Valid_When_Symptom_Is_Correct()
    {
        var dto = new SymptomDto("Fever");
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
