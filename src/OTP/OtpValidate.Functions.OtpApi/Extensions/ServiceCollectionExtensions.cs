using Application.Common.Interfaces;
using Application.Options;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OtpValidate.Functions.OtpApi.Services;

namespace OtpValidate.Functions.OtpApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddForValidateOtp(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IValidateOtpCommandFactory, ValidateOtpCommandFactory>();
        serviceCollection.AddScoped<IValidateOtpService, ValidateOtpService>();
        serviceCollection.Configure<OtpOptions>(configuration.GetSection("OtpOptions"));

        return serviceCollection;
    }
}