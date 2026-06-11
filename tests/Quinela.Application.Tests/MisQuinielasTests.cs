using Quinela.Application.Features.Quinielas;
using Quinela.Domain.Entities;
using Quinela.Infrastructure.Persistence;
using Quinela.Infrastructure.Repositories;

namespace Quinela.Application.Tests;

/// <summary>
/// Garantiza que un usuario solo "ve" las quinielas a las que tiene acceso en la
/// tabla usuarios_quinielas. El seed trae: Jason (user 1) -> 1,2,3; Elmer (user 2) -> solo 2 (Tegra).
/// </summary>
public class MisQuinielasTests
{
    private static GetMisQuinielasHandler Handler(QuinelaContext ctx, int? userId) =>
        new(new Repository<UsuarioQuiniela>(ctx), TestHelpers.CurrentUser("user", userId));

    [Fact]
    public async Task Elmer_SoloVe_LaQuinielaTegra()
    {
        using var ctx = TestHelpers.NewContext(nameof(Elmer_SoloVe_LaQuinielaTegra));

        var result = await Handler(ctx, userId: 2).Handle(new GetMisQuinielasQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { 2 }, result.Value.Select(x => x.Id).ToArray());
        Assert.Equal("Quiniela Tegra", result.Value.Single().Nombre);
    }

    [Fact]
    public async Task Jason_Ve_LasTresQuinielas()
    {
        using var ctx = TestHelpers.NewContext(nameof(Jason_Ve_LasTresQuinielas));

        var result = await Handler(ctx, userId: 1).Handle(new GetMisQuinielasQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { 1, 2, 3 }, result.Value.Select(x => x.Id).OrderBy(x => x).ToArray());
    }

    [Fact]
    public async Task UsuarioSinAccesos_NoVeNinguna()
    {
        using var ctx = TestHelpers.NewContext(nameof(UsuarioSinAccesos_NoVeNinguna));

        var result = await Handler(ctx, userId: 999).Handle(new GetMisQuinielasQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task SinUserIdEnElToken_NoVeNinguna()
    {
        using var ctx = TestHelpers.NewContext(nameof(SinUserIdEnElToken_NoVeNinguna));

        var result = await Handler(ctx, userId: null).Handle(new GetMisQuinielasQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task AccesoInactivo_NoSeIncluye()
    {
        using var ctx = TestHelpers.NewContext(nameof(AccesoInactivo_NoSeIncluye));
        // Elmer (2) gana un acceso INACTIVO a la quiniela 1; no debe verla.
        ctx.UsuariosQuinielas.Add(new UsuarioQuiniela
        {
            UserId = 2, QuinielaId = 1, Active = false, CreatedAt = DateTime.UtcNow, CreatedBy = "test"
        });
        ctx.SaveChanges();

        var result = await Handler(ctx, userId: 2).Handle(new GetMisQuinielasQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(new[] { 2 }, result.Value.Select(x => x.Id).ToArray());
    }
}
