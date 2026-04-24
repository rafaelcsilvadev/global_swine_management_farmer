namespace GlobalSwineManagementFarmer.Tests.PigBirth;

using GlobalSwineManagementFarmer.Api.src.PigBirth.DTOs;
using Xunit;

public class PigBirthDtoValidatorTests
{
    private readonly PigBirthDtoValidator _validator;

    public PigBirthDtoValidatorTests()
    {
        _validator = new PigBirthDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_LivesBirth_Is_Negative()
    {
        var dto = new PigBirthDto(-1, 0, 0, Guid.NewGuid(), Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "LivesBirth");
    }

    [Fact]
    public void Should_Have_Error_When_StillBorn_Is_Negative()
    {
        var dto = new PigBirthDto(5, -1, 0, Guid.NewGuid(), Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "StillBorn");
    }

    [Fact]
    public void Should_Have_Error_When_MummifiedBirth_Is_Negative()
    {
        var dto = new PigBirthDto(5, 0, -1, Guid.NewGuid(), Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "MummifiedBirth");
    }

    [Fact]
    public void Should_Have_Error_When_BatchId_Is_Empty()
    {
        var dto = new PigBirthDto(5, 0, 0, Guid.Empty, Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BatchId");
    }

    [Fact]
    public void Should_Have_Error_When_WarehouseId_Is_Empty()
    {
        var dto = new PigBirthDto(5, 0, 0, Guid.NewGuid(), Guid.Empty);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "WarehouseId");
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var dto = new PigBirthDto(10, 2, 1, Guid.NewGuid(), Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
