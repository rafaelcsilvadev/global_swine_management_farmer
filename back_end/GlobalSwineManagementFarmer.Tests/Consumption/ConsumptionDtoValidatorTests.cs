namespace GlobalSwineManagementFarmer.Tests.Consumption;

using GlobalSwineManagementFarmer.Api.src.Consumption.DTOs;
using Xunit;

public class ConsumptionDtoValidatorTests
{
    private readonly ConsumptionDtoValidator _validator;

    public ConsumptionDtoValidatorTests()
    {
        _validator = new ConsumptionDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_BagPetFood_Is_Negative()
    {
        var dto = new ConsumptionDto(-1, 0, Guid.NewGuid(), Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BagPetFood");
    }

    [Fact]
    public void Should_Have_Error_When_WaterTank_Is_Negative()
    {
        var dto = new ConsumptionDto(10, -1, Guid.NewGuid(), Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "WaterTank");
    }

    [Fact]
    public void Should_Have_Error_When_BatchId_Is_Empty()
    {
        var dto = new ConsumptionDto(10, 100, Guid.Empty, Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "BatchId");
    }

    [Fact]
    public void Should_Have_Error_When_WarehouseId_Is_Empty()
    {
        var dto = new ConsumptionDto(10, 100, Guid.NewGuid(), Guid.Empty);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "WarehouseId");
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var dto = new ConsumptionDto(10, 100, Guid.NewGuid(), Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
