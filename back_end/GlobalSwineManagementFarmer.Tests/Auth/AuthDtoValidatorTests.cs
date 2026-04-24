namespace GlobalSwineManagementFarmer.Tests.Auth;

using GlobalSwineManagementFarmer.Api.src.Auth.DTOs;
using FluentValidation.TestHelper;
using Xunit;
using System;

public class AuthDtoValidatorTests
{
    private readonly AuthDtoValidator _validator;

    public AuthDtoValidatorTests()
    {
        _validator = new AuthDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new AuthDto("", "Password123!", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new AuthDto("invalid-email", "Password123!", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Exceeds_20_Characters()
    {
        var model = new AuthDto("verylongemailaddress@test.com", "Password123!", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new AuthDto("test@test.com", "", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var model = new AuthDto("test@test.com", "Ps1!", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Exceeds_16_Characters()
    {
        var model = new AuthDto("test@test.com", "Password123!ExtraLong", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Missing_Uppercase()
    {
        var model = new AuthDto("test@test.com", "password123!", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Have_Error_When_RuleId_Is_Empty_Guid()
    {
        var model = new AuthDto("test@test.com", "Password123!", Guid.Empty);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.RuleId);
    }

    [Theory]
    [InlineData("admin' --")]
    [InlineData("admin'; DROP TABLE Users; --")]
    [InlineData("' OR '1'='1")]
    public void Should_Have_Error_When_Email_Contains_SQL_Injection_Patterns(string injectionEmail)
    {
        var model = new AuthDto(injectionEmail, "Password123!", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Theory]
    [InlineData("pass' --")]
    [InlineData("' OR '1'='1")]
    public void Should_Handle_SQL_Injection_Patterns_In_Password(string injectionPassword)
    {
        var model = new AuthDto("t@t.com", injectionPassword, Guid.NewGuid());
        var result = _validator.TestValidate(model);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new AuthDto("t@t.com", "Pass123!", Guid.NewGuid());
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
