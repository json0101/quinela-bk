using Microsoft.EntityFrameworkCore;
using Moq;
using Quinela.Application.Common.Abstractions;
using Quinela.Infrastructure.Persistence;
using UserApp.Service.Services.Users;
using UserApp.Service.Services.Users.Dto;

namespace Quinela.Application.Tests;

/// <summary>Utilidades compartidas para construir un contexto en memoria y dobles de prueba.</summary>
internal static class TestHelpers
{
    // Cada test usa una base en memoria aislada (nombre único) para no contaminarse entre sí.
    public static QuinelaContext NewContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<QuinelaContext>()
            .UseInMemoryDatabase(dbName)
            .EnableSensitiveDataLogging()
            .Options;

        var ctx = new QuinelaContext(options);
        ctx.Database.EnsureCreated(); // aplica los seed (equipos, grupos, partidos, tipo_partido).
        return ctx;
    }

    public static ICurrentUser CurrentUser(string userName)
    {
        var mock = new Mock<ICurrentUser>();
        mock.SetupGet(x => x.UserName).Returns(userName);
        return mock.Object;
    }

    // IUserService que reconoce únicamente los usernames indicados como existentes en UserApp.
    public static IUserService UserService(params string[] existingUserNames)
    {
        var resume = existingUserNames
            .Select((name, i) => new UserResumeDto { Id = i + 1, UserName = name })
            .ToList();

        var mock = new Mock<IUserService>();
        mock.Setup(x => x.GetResume()).Returns(resume);
        return mock.Object;
    }
}
