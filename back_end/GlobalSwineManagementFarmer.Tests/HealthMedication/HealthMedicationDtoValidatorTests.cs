namespace GlobalSwineManagementFarmer.Tests.HealthMedication;

using GlobalSwineManagementFarmer.Api.src.HealthMedication.DTOs;
using Xunit;
using System;
using System.Collections.Generic;

public class HealthMedicationDtoValidatorTests
{
    private readonly HealthMedicationDtoValidator _validator;

    public HealthMedicationDtoValidatorTests()
    {
        _validator = new HealthMedicationDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_BatchId_Is_Empty()
    {
        var dto = new HealthMedicationDto(Guid.Empty, Guid.NewGuid(), null, "Effective", new List<Guid> { Guid.NewGuid() });
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BatchId");
    }

    [Fact]
    public void Should_Have_Error_When_WarehouseId_Is_Empty()
    {
        var dto = new HealthMedicationDto(Guid.NewGuid(), Guid.Empty, null, "Effective", new List<Guid> { Guid.NewGuid() });
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "WarehouseId");
    }

    [Fact]
    public void Should_Have_Error_When_TreatmentEfficacy_Is_Empty()
    {
        var dto = new HealthMedicationDto(Guid.NewGuid(), Guid.NewGuid(), null, "", new List<Guid> { Guid.NewGuid() });
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "TreatmentEfficacy");
    }

    [Fact]
    public void Should_Have_Error_When_SymptomIds_Is_Empty()
    {
        var dto = new HealthMedicationDto(Guid.NewGuid(), Guid.NewGuid(), null, "Effective", new List<Guid>());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "SymptomIds");
    }

    [Fact]
    public void Should_Have_Error_When_TreatmentEfficacy_Is_Too_Long()
    {
        var dto = new HealthMedicationDto(Guid.NewGuid(), Guid.NewGuid(), null, new string('A', 501), new List<Guid> { Guid.NewGuid() });
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "TreatmentEfficacy");
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var dto = new HealthMedicationDto(Guid.NewGuid(), Guid.NewGuid(), "http://media.com/image.jpg", "Effective", new List<Guid> { Guid.NewGuid() });
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
