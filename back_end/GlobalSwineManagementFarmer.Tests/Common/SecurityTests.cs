namespace GlobalSwineManagementFarmer.Tests.Common;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Xunit;
using GlobalSwineManagementFarmer.Api.src.Auth.Controllers;
using GlobalSwineManagementFarmer.Api.src.Batch.Controllers;
using GlobalSwineManagementFarmer.Api.src.Consumption.Controllers;
using GlobalSwineManagementFarmer.Api.src.HealthMedication.Controllers;
using GlobalSwineManagementFarmer.Api.src.PigBirth.Controllers;
using GlobalSwineManagementFarmer.Api.src.Symptom.Controllers;
using GlobalSwineManagementFarmer.Api.src.Warehouse.Controllers;

public class SecurityTests
{
    [Theory]
    [InlineData(typeof(BatchController))]
    [InlineData(typeof(ConsumptionController))]
    [InlineData(typeof(HealthMedicationController))]
    [InlineData(typeof(PigBirthController))]
    [InlineData(typeof(SymptomController))]
    [InlineData(typeof(WarehouseController))]
    public void Controller_Should_Have_Authorize_Attribute(Type controllerType)
    {
        var attribute = controllerType.GetCustomAttribute<AuthorizeAttribute>();
        Assert.NotNull(attribute);
    }

    [Fact]
    public void AuthController_Should_Have_AllowAnonymous_On_SignIn()
    {
        var method = typeof(AuthController).GetMethods()
            .FirstOrDefault(m => m.Name == "SignIn");
        Assert.NotNull(method);
        var attribute = method.GetCustomAttribute<AllowAnonymousAttribute>();
        Assert.NotNull(attribute);
    }
}
