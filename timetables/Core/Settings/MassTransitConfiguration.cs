
using Contracts.Models;
using MassTransit;

namespace Timetables.Core.Settings;

public static class MassTransitConfiguration
{
    public static void ConfigureMassTransit(IServiceCollection services)
    {
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context, KebabCaseEndpointNameFormatter.Instance);
            });

            config.AddLogging();
            config.AddRequestClient<DoctorRequest>(
                destinationAddress: new Uri("queue:doctor-request-queue"),
                timeout: RequestTimeout.After(s: 30));
        });

        services.AddMassTransitHostedService();
    }
}