using System.Linq;
using Infrastructure.Persistence.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OtpAccess.Functions.OtpApi.Extensions;

var host = new HostBuilder()
           .ConfigureFunctionsWebApplication()
           .ConfigureServices(
               (context, services) =>
               {
                   var configuration = context.Configuration;

                   services.AddApplicationInsightsTelemetryWorkerService();
                   services.ConfigureFunctionsApplicationInsights();

                   services.Configure<LoggerFilterOptions>(
                       options =>
                       {
                           var toRemove = options.Rules
                                                 .FirstOrDefault(rule => rule.ProviderName == "Microsoft.Extensions.Logging.ApplicationInsights.ApplicationInsightsLoggerProvider");

                           if (toRemove is not null)
                           {
                               options.Rules.Remove(toRemove);
                           }
                       });

                   services.AddInfrastructureServices(configuration);
                   services.AddForGenerateOtp(configuration);
               })
           .Build();

host.Run();