// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using DependencyInjection;
using Escendit.Extensions.Hosting.Abstractions;
using Escendit.Extensions.Hosting.Exceptions;
using Escendit.Extensions.Hosting.Validators;
using Temporalio.Client;
using Temporalio.Extensions.Hosting;
using Temporalio.Extensions.OpenTelemetry;

/// <summary>
/// Provides extension methods for configuring and customizing the <see cref="HostApplicationBuilder"/>.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    private const string DefaultTemporalServiceName = "temporal";

    /// <summary>
    /// Adds a Temporal worker service to the application builder.
    /// </summary>
    /// <param name="builder">The application <see cref="HostApplicationBuilder"/> to be configured.</param>
    /// <param name="serviceName">The name of the Temporal service. Defaults to the internal default service name.</param>
    /// <param name="buildId">The optional build ID for the Temporal worker.</param>
    /// <returns>An instance of <see cref="ITemporalWorkerServiceOptionsBuilder"/> for further configuration.</returns>
    public static ITemporalWorkerServiceOptionsBuilder AddTemporalWorkerService(
        this HostApplicationBuilder builder,
        string serviceName = DefaultTemporalServiceName,
        string? buildId = null)
    {
        return builder
            .AddTemporalWorkerService(_ => { }, serviceName, buildId);
    }

    /// <summary>
    /// Adds a Temporal worker service to the application builder with optional configuration settings.
    /// </summary>
    /// <param name="builder">The application <see cref="HostApplicationBuilder"/> to configure.</param>
    /// <param name="configureOptions">An action to configure <see cref="TemporalOptions"/> for the Temporal worker.</param>
    /// <param name="serviceName">The name of the Temporal service. Defaults to the internal default service name.</param>
    /// <param name="buildId">The optional build ID for the Temporal worker instance.</param>
    /// <returns>An instance of <see cref="ITemporalWorkerServiceOptionsBuilder"/> for further configuration.</returns>
    public static ITemporalWorkerServiceOptionsBuilder AddTemporalWorkerService(
        this HostApplicationBuilder builder,
        Action<TemporalOptions> configureOptions,
        string serviceName = DefaultTemporalServiceName,
        string? buildId = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configureOptions);

        var temporalOptions = builder
            .Configuration
            .GetRequiredSection($"Services:{serviceName}")
            .Get<TemporalOptions>();
        var temporalHost = temporalOptions.Grpc.First();

        return builder
            .AddTemporalClient(options =>
            {
                options.TargetHost = $"{temporalHost.Host}:{temporalHost.Port}";
                options.Namespace = temporalOptions.Namespace;
                options.Interceptors = [new TracingInterceptor()];
            })
            .Services
            .AddHostedTemporalWorker(temporalOptions.Queue, buildId);
    }

    /// <summary>
    /// Adds a Temporal client to the application builder.
    /// </summary>
    /// <param name="builder">The application <see cref="HostApplicationBuilder"/> to be configured.</param>
    /// <param name="serviceName">The name of the Temporal service. Defaults to the internal default service name.</param>
    /// <returns>The modified <see cref="HostApplicationBuilder"/> instance.</returns>
    public static HostApplicationBuilder AddTemporalClient(
        this HostApplicationBuilder builder,
        string serviceName = DefaultTemporalServiceName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(serviceName);
        builder
            .AddTemporalClient(_ => { }, serviceName);
        return builder;
    }

    /// <summary>
    /// Adds a Temporal client to the application builder.
    /// </summary>
    /// <param name="builder">The application <see cref="HostApplicationBuilder"/> to be configured.</param>
    /// <param name="configureOptions">An action to configure the <see cref="TemporalClientConnectOptions"/>.</param>
    /// <param name="serviceName">The name of the Temporal service. Defaults to the internal default service name.</param>
    /// <returns>The <see cref="HostApplicationBuilder"/> instance for further configuration.</returns>
    public static HostApplicationBuilder AddTemporalClient(
        this HostApplicationBuilder builder,
        Action<TemporalClientConnectOptions> configureOptions,
        string serviceName = DefaultTemporalServiceName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(configureOptions);
        ArgumentNullException.ThrowIfNull(serviceName);

        var temporalOptions = builder
            .Configuration
            .GetRequiredSection($"Services:{serviceName}")
            .Get<TemporalOptions>() ?? throw new ConfigurationMissingException($"Missing configuration for Temporal service '{serviceName}'.");

        var temporalHost = temporalOptions.Grpc.First();

        builder
            .Services
            .ConfigureOptions<TemporalOptionsValidator>()
            .AddTemporalClient($"{temporalHost.Host}:{temporalHost.Port}")
            .Configure(configureOptions);
        return builder;
    }
}
