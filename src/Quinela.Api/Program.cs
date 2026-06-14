using Microsoft.EntityFrameworkCore;
using Quinela.Infrastructure.Persistence;
using Quinela.Api.Common;
using Quinela.Application.Common;
using Quinela.Application.Common.Abstractions;
using Quinela.Application.Commons;
using Quinela.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authorization;
using System.Threading.RateLimiting;
using UserApp.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSetting>(builder.Configuration.GetSection("AppSetting"));

// --- Conexiones a base de datos ---
// Quinela escribe en su propia base 'quinela'.
var quinelaConnectionString = builder.Configuration.GetConnectionString("Quinela");
builder.Services.AddDbContext<QuinelaContext>(options => options.UseNpgsql(quinelaConnectionString));

// ...y administra 'userapp' (usuarios/roles/screens). Quinela es dueña de su esquema
// vía migraciones EF hospedadas en Quinela.Infrastructure.
var userAppConnectionString = builder.Configuration.GetConnectionString("UserApp");
builder.Services.AddDbContext<UserAppContext>(options =>
    options.UseNpgsql(userAppConnectionString, b => b.MigrationsAssembly("Quinela.Infrastructure")));

// --- Integracion con UserApp (registro/consulta de usuarios) ---
// El 3er arg (applicationId) fija ApplicationGlobal.ApplicationGlobalID, que GetMenu usa
// para filtrar las screens. Debe ser 2: la app "Quinela" del seed (application_id = 2).
// UserApp es la app 1 (la administra su propio frontend/API con global = 1).
var autoMapperLicenseKey = builder.Configuration["AppSetting:AutoMapperLicence"];
UserApp.Service.Main.ConfigureService(builder.Services, null, 2, autoMapperLicenseKey ?? "");

// --- Autenticacion JWT (Quinela emite y valida su propio token) ---
string jwtSecret = builder.Configuration["AppSetting:JwtSecret"] ?? "";
string jwtIssuer = builder.Configuration["AppSetting:JwtIssuer"] ?? "";
string jwtAudience = builder.Configuration["AppSetting:JwtAudience"] ?? "";
builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ClockSkew = TimeSpan.Zero
    };
});

// Defensa en profundidad: TODO endpoint exige autenticacion por defecto.
// Solo lo que tenga [AllowAnonymous] queda publico (hoy: el login).
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// --- Application (MediatR + validaciones) ---
builder.Services.AddApplication();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUserService>();

// Repositorio genérico + unidad de trabajo (sobre QuinelaContext).
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Servicio de recálculo de grupos + ranking (se dispara al cambiar el estado de un partido).
builder.Services.AddScoped<IRankingService, Quinela.Application.Features.Ranking.RankingService>();

// --- Automatización de partidos (worldcup26.ir) ---
// Cliente HTTP tipado del API externo.
var worldCupApiBaseUrl = builder.Configuration["AppSetting:WorldCupApiBaseUrl"] ?? "https://worldcup26.ir";
builder.Services.AddHttpClient<Quinela.Application.Features.AutomationMatch.IWorldCupApiClient,
    Quinela.Application.Features.AutomationMatch.WorldCupApiClient>(client =>
{
    client.BaseAddress = new Uri(worldCupApiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(15);
});
// Servicios hermanos del feature AutomationMatch.
builder.Services.AddScoped<Quinela.Application.Features.AutomationMatch.MatchStartVerificationService>();
builder.Services.AddScoped<Quinela.Application.Features.AutomationMatch.MatchStatusVerificationService>();
// Orquestador en background (cada 30s); se apaga con AppSetting:AutomationMatchEnabled = false.
builder.Services.AddHostedService<Quinela.Api.AutomationMatch.AutomationMatchHostedService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rate limiting: frena fuerza bruta en el login (politica "login").
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddPolicy("login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

// Detras de un proxy (Caddy): leer X-Forwarded-For para ver la IP real del cliente.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

// Migraciones de Quinela: INTENCIONALES, no automaticas. Solo si ApplyMigrations=true.
if (builder.Configuration.GetValue<bool>("ApplyMigrations"))
{
    using var scope = app.Services.CreateScope();
    // userapp primero (usuarios/roles/screens), luego la base de Quinela.
    scope.ServiceProvider.GetRequiredService<UserAppContext>().Database.Migrate();
    scope.ServiceProvider.GetRequiredService<QuinelaContext>().Database.Migrate();
}

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
