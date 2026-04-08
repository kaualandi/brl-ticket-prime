using Microsoft.Data.SqlClient;
using System.Data;
using TicketPrime.Api.Features.Eventos;
using TicketPrime.Api.Features.Usuarios;

namespace TicketPrime.Api.Infrastructure;

public static class ServiceExtensions
{
    public static IServiceCollection AddDatabase(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IDbConnection>(_ =>
            new SqlConnection(config.GetConnectionString("DefaultConnection")));

        return services;
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IEventoRepository, EventoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        return services;
    }

    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowBlazorClient", policy =>
                policy.WithOrigins(
                        "http://localhost:5173",
                        "https://localhost:5173",
                        "http://localhost:5272")
                      .AllowAnyHeader()
                      .AllowAnyMethod());
        });

        return services;
    }
}
