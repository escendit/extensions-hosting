// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using DependencyInjection;
using Orleans.Configuration;

/// <summary>
/// Provides extension methods for configuring and enhancing server-related functionality
/// within the Microsoft.Extensions.Hosting namespace.
/// </summary>
public static class ServerExtensions
{
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Configures the host application builder to use a runtime server.
        /// This method extends the HostApplicationBuilder with functionality
        /// related to setting up a runtime server during the application configuration.
        /// </summary>
        /// <returns>
        /// The configured <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        public HostApplicationBuilder AddOrleansServerRuntime()
        {
            ArgumentNullException.ThrowIfNull(builder);

            // Setup Orleans Defaults
            return builder
                .AddServerClusteringDefaults()
                .AddEventSourcingDefaults()
                .AddHostingDefaults()
                .AddServerActivityPropagation()
                .AddServiceDefaults(
                    tracingProvider => tracingProvider
                        .AddSource(OrleansApplicationSourceName),
                    metersProviderBuilder: metricsProviderBuilder => metricsProviderBuilder
                        .AddMeter(OrleansApplicationMeterName));
        }

        /// <summary>
        /// Configures the host application builder to use a runtime server.
        /// This method extends the HostApplicationBuilder with functionality
        /// related to setting up a runtime server during the application configuration.
        /// </summary>
        /// <param name="configureAction">
        /// The <see cref="HostApplicationBuilder"/> instance to configure.
        /// </param>
        /// <returns>
        /// The configured <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        public HostApplicationBuilder AddOrleansServerRuntime(Action<ISiloBuilder> configureAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configureAction);
            builder.UseOrleans(configureAction);
            return builder;
        }

        /// <summary>
        /// Configures the host application builder with default clustering settings for Orleans.
        /// This method sets up clustering configurations by binding them to the application's
        /// configuration and applies them to the Orleans silo builder.
        /// </summary>
        /// <returns>
        /// The configured <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the provided <see cref="HostApplicationBuilder"/> instance is null.
        /// </exception>
        private HostApplicationBuilder AddServerClusteringDefaults()
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder
                .UseOrleans(siloBuilder => siloBuilder
                    .ConfigureServices(services => services
                        .Configure<ClusterOptions>(configureOptions => builder
                            .Configuration
                            .Bind(configureOptions))));
        }

        /// <summary>
        /// Configures the host application builder with default settings
        /// for event sourcing functionality. This method adds the necessary
        /// components and configurations to enable event sourcing in the application.
        /// </summary>
        /// <returns>
        /// The configured <see cref="HostApplicationBuilder"/> instance with
        /// event sourcing defaults applied.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the provided builder instance is null.
        /// </exception>
        private HostApplicationBuilder AddEventSourcingDefaults()
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder
                .UseOrleans(siloBuilder => siloBuilder
                    .AddLogStorageBasedLogConsistencyProviderAsDefault());
        }

        /// <summary>
        /// Configures the host application builder with default settings for hosting.
        /// This method integrates necessary configurations for hosting modes, enabling
        /// features such as Kubernetes-specific hosting when applicable.
        /// </summary>
        /// <returns>
        /// The configured <see cref="HostApplicationBuilder"/> instance with hosting defaults applied.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the <see cref="HostApplicationBuilder"/> instance is null.
        /// </exception>
        private HostApplicationBuilder AddHostingDefaults()
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder
                .UseOrleans(siloBuilder =>
                {
                    var hostingMode = builder
                        .Configuration
                        .GetValue<string>("Hosting");

                    _ = hostingMode switch
                    {
                        "Kubernetes" => siloBuilder.UseKubernetesHosting(),
                        _ => siloBuilder,
                    };
                });
            return builder;
        }

        private HostApplicationBuilder AddServerActivityPropagation()
        {
            return builder
                .UseOrleans(siloBuilder => siloBuilder
                    .AddActivityPropagation());
        }
    }

    private const string OrleansApplicationSourceName = "Microsoft.Orleans.Application";
    private const string OrleansApplicationMeterName = "Microsoft.Orleans";
}
