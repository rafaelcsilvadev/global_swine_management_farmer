namespace GlobalSwineManagementFarmer.Tests.Warehouse;

using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using Xunit;

public class WarehouseDtoValidatorTests
{
    private readonly WarehouseDtoValidator _validator;

    public WarehouseDtoValidatorTests()
    {
        _validator = new WarehouseDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Empty()
    {
        var dto = new WarehouseDto("", "Galpão de Engorda");
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Code");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var dto = new WarehouseDto("W001", "");
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var dto = new WarehouseDto("W001", "Galpão de Engorda");
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
