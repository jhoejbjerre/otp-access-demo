using Domain.Interfaces;

using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string? otpDatabaseConnection = Environment.GetEnvironmentVariable("OtpDatabaseConnection");

        _ = services.AddDbContext<OtpDbContext>(
            (serviceProvider, options) =>
            {
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                _ = options.UseAzureSql(otpDatabaseConnection)
                       .EnableSensitiveDataLogging()
                       .UseLoggerFactory(loggerFactory)
                       .LogTo(Console.WriteLine, LogLevel.Information)
                       .EnableDetailedErrors();
            });

        _ = services.AddScoped<IOtpRequestRepository, OtpRequestRepository>();

        return services;
    }
}
