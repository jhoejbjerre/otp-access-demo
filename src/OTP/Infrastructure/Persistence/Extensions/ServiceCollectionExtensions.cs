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
        var otpDatabaseConnection = Environment.GetEnvironmentVariable("OtpDatabaseConnection");

        services.AddDbContext<OtpDbContext>(
            (serviceProvider, options) =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                options.UseAzureSql(otpDatabaseConnection)
                       .EnableSensitiveDataLogging()
                       .UseLoggerFactory(loggerFactory)
                       .LogTo(Console.WriteLine, LogLevel.Information)
                       .EnableDetailedErrors();
            });

        services.AddScoped<IOtpRequestRepository, OtpRequestRepository>();

        return services;
    }
}
