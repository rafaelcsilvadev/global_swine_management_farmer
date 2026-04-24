namespace GlobalSwineManagementFarmer.Tests.WarehouseTitle;

using GlobalSwineManagementFarmer.Api.src.Warehouse.DTOs;
using Xunit;

public class WarehouseTitleDtoValidatorTests
{
    private readonly WarehouseTitleDtoValidator _validator;

    public WarehouseTitleDtoValidatorTests()
    {
        _validator = new WarehouseTitleDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var dto = new WarehouseTitleDto(null, "");
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Too_Long()
    {
        var dto = new WarehouseTitleDto(null, new string('A', 101));
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Should_Be_Valid_When_Title_Is_Correct()
    {
        var dto = new WarehouseTitleDto(null, "Fattening Warehouse");
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
