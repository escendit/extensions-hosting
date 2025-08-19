// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using System.Globalization;
using Configuration;
using DependencyInjection;
using Escendit.Extensions.Hosting.Abstractions;
using Escendit.Extensions.Hosting.Exceptions;

/// <summary>
/// Provides extension methods for configuring and customizing the <see cref="HostApplicationBuilder"/>.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    private const string DefaultTemporalServiceName = "temporal";

    /// <summary>
    /// Adds a Temporal hosted service to the application builder with specified configuration settings.
    /// </summary>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> used to configure the application.</param>
    /// <param name="name">The name of the Temporal service to configure.</param>
    /// <param name="configureOptions">An optional action to configure the <see cref="ITemporalBuilder"/> for the hosted service.</param>
    /// <returns>The <see cref="HostApplicationBuilder"/> instance for further configuration.</returns>
    public static HostApplicationBuilder AddTemporalHostedService(
        this HostApplicationBuilder builder,
        string name,
        Action<ITemporalBuilder> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(configureOptions);

        var section = builder.Configuration.GetRequiredSection($"Services:{name}").Get<TemporalOptions>();

        if (!(section?.Grpc.Any() ?? false))
        {
            throw new ConfigurationMissingException($"Missing configuration for Temporal service '{name}'.");
        }

        var temporalHost = section.Grpc.First();

        var internalBuilder = builder
            .Services
            .AddTemporalHostedService(
                section.Queue,
                string.Create(CultureInfo.InvariantCulture, $"{temporalHost.Host}:{temporalHost.Port}"),
                section.Namespace,
                section.BuildId);

        configureOptions.Invoke(internalBuilder);
        return builder;
    }

    /// <summary>
    /// Adds a Temporal hosted service to the application builder using the default service name and optional configuration settings.
    /// </summary>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> used to configure the application.</param>
    /// <param name="configureOptions">An optional action to configure the <see cref="ITemporalBuilder"/> for the hosted service.</param>
    /// <returns>The <see cref="HostApplicationBuilder"/> instance for further configuration.</returns>
    public static HostApplicationBuilder AddTemporalHostedService(
        this HostApplicationBuilder builder,
        Action<ITemporalBuilder> configureOptions)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configureOptions);
        return AddTemporalHostedService(builder, DefaultTemporalServiceName, configureOptions);
    }

    /// <summary>
    /// Adds a Temporal hosted service to the application builder with specified configuration settings.
    /// </summary>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> used to configure the application.</param>
    /// <returns>The <see cref="HostApplicationBuilder"/> instance for further configuration.</returns>
    public static HostApplicationBuilder AddTemporalHostedService(
        this HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return AddTemporalHostedService(builder, _ => { });
    }

    /// <summary>
    /// Adds a keyed Temporal client to the application builder using the specified client name and namespace.
    /// </summary>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> used to configure the application.</param>
    /// <param name="clientName">The name of the Temporal client to configure.</param>
    /// <param name="clientNamespace">The namespace of the Temporal client.</param>
    /// <returns>The <see cref="HostApplicationBuilder"/> instance for further configuration.</returns>
    /// <exception cref="ConfigurationMissingException">Thrown if the required configuration for the Temporal client is not found.</exception>
    public static HostApplicationBuilder AddKeyedTemporalClient(
        this HostApplicationBuilder builder,
        string clientName,
        string clientNamespace)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(clientName);
        ArgumentNullException.ThrowIfNull(clientNamespace);

        var section = builder.Configuration.GetRequiredSection($"Services:{clientName}").Get<TemporalOptions>();

        if (!(section?.Grpc.Any() ?? false))
        {
            throw new ConfigurationMissingException($"Missing configuration for Temporal client '{clientName}'.");
        }

        var temporalHost = section.Grpc.First();

        builder
            .Services
            .AddKeyedTemporalClient(clientName, temporalHost.Authority, clientNamespace);
        return builder;
    }
}
