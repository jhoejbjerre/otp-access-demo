using Application.Interfaces;
using Application.Options;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OtpAccess.Functions.OtpApi.Services;

namespace OtpAccess.Functions.OtpApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForGenerateOtp(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IGenerateOtpCommandFactory, GenerateOtpCommandFactory>();
        serviceCollection.AddScoped<IGenerateOtpService, GenerateOtpService>();
        serviceCollection.AddScoped<IValidateOtpService, ValidateOtpService>();

        serviceCollection.Configure<OtpOptions>(configuration.GetSection("OtpOptions"));

        return serviceCollection;
    }
}