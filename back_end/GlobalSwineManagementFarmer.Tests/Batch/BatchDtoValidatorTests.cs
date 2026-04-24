namespace GlobalSwineManagementFarmer.Tests.Batch;

using GlobalSwineManagementFarmer.Api.src.Batch.DTOs;
using Xunit;

public class BatchDtoValidatorTests
{
    private readonly BatchDtoValidator _validator;

    public BatchDtoValidatorTests()
    {
        _validator = new BatchDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Code_Is_Empty()
    {
        var dto = new BatchDto("", 10, Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Code");
    }

    [Fact]
    public void Should_Have_Error_When_DaysLife_Is_Negative()
    {
        var dto = new BatchDto("LOTE001", -1, Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "DaysLife");
    }

    [Fact]
    public void Should_Have_Error_When_WarehouseId_Is_Empty()
    {
        var dto = new BatchDto("LOTE001", 10, Guid.Empty);
        var result = _validator.Validate(dto);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "WarehouseId");
    }

    [Fact]
    public void Should_Be_Valid_When_All_Fields_Are_Correct()
    {
        var dto = new BatchDto("LOTE001", 10, Guid.NewGuid());
        var result = _validator.Validate(dto);
        Assert.True(result.IsValid);
    }
}
