// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using DependencyInjection;
using Orleans.Configuration;

/// <summary>
/// A class that provides extension methods for the HostApplicationBuilder.
/// Contains methods to extend the functionality of the host application builder
/// during application configuration and setup.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the host application builder to use a runtime server.
    /// This method extends the HostApplicationBuilder with functionality
    /// related to setting up a runtime server during the application configuration.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="HostApplicationBuilder"/> instance to configure.
    /// </param>
    /// <returns>
    /// The configured <see cref="HostApplicationBuilder"/> instance.
    /// </returns>
    public static HostApplicationBuilder UseRuntimeServer(this HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder
            .AddEventSourcingDefaults()
            .AddHostingDefaults()
            .AddActivityPropagation();
    }

    private static HostApplicationBuilder AddEventSourcingDefaults(this HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder
            .UseOrleans(siloBuilder => siloBuilder
                .AddLogStorageBasedLogConsistencyProviderAsDefault()
                .AddStateStorageBasedLogConsistencyProviderAsDefault());
    }

    private static HostApplicationBuilder AddHostingDefaults(this HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder
            .Services
            .Configure<ClusterOptions>(configureOptions => builder
                .Configuration
                .Bind(configureOptions));

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

    private static HostApplicationBuilder AddActivityPropagation(this HostApplicationBuilder builder)
    {
        return builder
            .UseOrleans(siloBuilder => siloBuilder
                .AddActivityPropagation());
    }
}
