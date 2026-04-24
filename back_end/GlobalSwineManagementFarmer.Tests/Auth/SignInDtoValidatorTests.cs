namespace GlobalSwineManagementFarmer.Tests.Auth;

using FluentValidation.TestHelper;
using GlobalSwineManagementFarmer.Api.src.Auth.DTOs;
using Xunit;

public class SignInDtoValidatorTests
{
    private readonly SignInDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new SignInDto("", "Password123!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Longer_Than_20()
    {
        var model = new SignInDto("verylongemailaddress@test.com", "Password123!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Contains_SQL_Injection()
    {
        var model = new SignInDto("admin' OR 1=1--", "Password123!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new SignInDto("test@test.com", "");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var model = new SignInDto("test@test.com", "abc");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Long()
    {
        var model = new SignInDto("test@test.com", "Password1234567890!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Missing_Uppercase()
    {
        var model = new SignInDto("test@test.com", "password123!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Missing_Lowercase()
    {
        var model = new SignInDto("test@test.com", "PASSWORD123!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Missing_Number()
    {
        var model = new SignInDto("test@test.com", "Password!");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Missing_SpecialChar()
    {
        var model = new SignInDto("test@test.com", "Password123");
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new SignInDto("test@test.com", "Pass123!");
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
