// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using DependencyInjection;
using Orleans.Configuration;

/// <summary>
/// Provides extension methods for the <c>HostApplicationBuilder</c> class,
/// enabling additional functionality for configuring and building host applications.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the <c>HostApplicationBuilder</c> with default clustering settings,
    /// including setting up Orleans cluster options and binding them to the application's configuration.
    /// </summary>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> instance to configure.</param>
    /// <returns>The configured <see cref="HostApplicationBuilder"/>.</returns>
    private static HostApplicationBuilder AddClusteringDefaults(this HostApplicationBuilder builder)
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
    /// Configures the <c>HostApplicationBuilder</c> with default event sourcing settings,
    /// including adding Log Storage-based log consistency provider as the default provider.
    /// </summary>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> instance to configure.</param>
    /// <returns>The configured <see cref="HostApplicationBuilder"/>.</returns>
    private static HostApplicationBuilder AddEventSourcingDefaults(this HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return builder
            .UseOrleans(siloBuilder => siloBuilder
                .AddLogStorageBasedLogConsistencyProviderAsDefault());
    }

    /// <summary>
    /// Configures the <c>HostApplicationBuilder</c> with default hosting settings,
    /// including binding configuration options and setting up Orleans hosting features.
    /// </summary>
    /// <example>
    /// <para>In the case of setting Hosting to Kubernetes in the below example.</para>
    /// <code>
    /// {
    ///     "Hosting": "Kubernetes"
    /// }
    /// </code>
    /// </example>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> instance to configure.</param>
    /// <returns>The configured <see cref="HostApplicationBuilder"/>.</returns>
    private static HostApplicationBuilder AddHostingDefaults(this HostApplicationBuilder builder)
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

    /// <summary>
    /// Configures the <c>HostApplicationBuilder</c> to use activity propagation
    /// by setting up the necessary Orleans Silo configurations.
    /// </summary>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> instance to configure.</param>
    /// <returns>The configured <see cref="HostApplicationBuilder"/>.</returns>
    private static HostApplicationBuilder AddActivityPropagation(this HostApplicationBuilder builder)
    {
        return builder
            .UseOrleans(siloBuilder => siloBuilder
                .AddActivityPropagation());
    }
}
