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
        _ = serviceCollection.AddScoped<IValidateOtpCommandFactory, ValidateOtpCommandFactory>();
        _ = serviceCollection.AddScoped<IValidateOtpService, ValidateOtpService>();
        _ = serviceCollection.Configure<OtpOptions>(configuration.GetSection("OtpOptions"));

        return serviceCollection;
    }
}
