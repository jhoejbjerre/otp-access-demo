using System.Linq;

using Infrastructure.Persistence.Extensions;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using OtpAccess.Functions.OtpApi.Extensions;

IHost host = new HostBuilder()
           .ConfigureFunctionsWebApplication()
           .ConfigureServices(
               (context, services) =>
               {
                   Microsoft.Extensions.Configuration.IConfiguration configuration = context.Configuration;

                   _ = services.AddApplicationInsightsTelemetryWorkerService();
                   _ = services.ConfigureFunctionsApplicationInsights();

                   _ = services.Configure<LoggerFilterOptions>(
                       options =>
                       {
                           LoggerFilterRule toRemove = options.Rules
                                                 .FirstOrDefault(rule => rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");

                           if (toRemove is not null)
                           {
                               _ = options.Rules.Remove(toRemove);
                           }
                       });

                   _ = services.AddInfrastructureServices(configuration);
                   _ = services.AddForGenerateOtp(configuration);
               })
           .Build();

host.Run();
