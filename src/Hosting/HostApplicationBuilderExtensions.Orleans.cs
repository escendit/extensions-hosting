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
    private const string OrleansApplicationSourceName = "Microsoft.Orleans.Application";
    private const string OrleansApplicationMeterName = "Microsoft.Orleans";

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
    public static HostApplicationBuilder AddOrleansServerRuntime(this HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        // Setup Orleans Defaults
        return builder
            .AddClusteringDefaults()
            .AddEventSourcingDefaults()
            .AddHostingDefaults()
            .AddActivityPropagation()
            .AddServiceDefaults(
                tracingProvider => tracingProvider
                    .AddSource(OrleansApplicationSourceName),
                metersProviderBuilder: metricsProviderBuilder => metricsProviderBuilder
                    .AddMeter(OrleansApplicationMeterName));
    }
}
