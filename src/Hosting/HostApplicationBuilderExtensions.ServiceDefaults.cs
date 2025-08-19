// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using DependencyInjection;
using Diagnostics.HealthChecks;
using Logging;
using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

/// <summary>
/// Provides extension methods for configuring and extending the behavior
/// of <see cref="HostApplicationBuilder"/>.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    /// <summary>
    /// Adds default service configurations and behaviors to the specified <see cref="HostApplicationBuilder"/>.
    /// This includes settings such as OpenTelemetry instrumentation, health checks,
    /// HTTP client configurations, service discovery, and resilience mechanisms.
    /// </summary>
    /// <param name="builder">The <see cref="HostApplicationBuilder"/> to configure.</param>
    /// <param name="tracerProviderBuilder">
    /// Optional function for configuring the OpenTelemetry <see cref="TracerProviderBuilder"/>.
    /// If null, no custom tracer configurations will be applied.
    /// </param>
    /// <param name="metersProviderBuilder">
    /// Optional function for configuring the OpenTelemetry <see cref="MeterProviderBuilder"/>.
    /// If null, no custom meter configurations will be applied.
    /// </param>
    /// <returns>The modified <see cref="HostApplicationBuilder"/> instance.</returns>
    public static HostApplicationBuilder AddServiceDefaults(
        this HostApplicationBuilder builder,
        Func<TracerProviderBuilder, TracerProviderBuilder>? tracerProviderBuilder = null,
        Func<MeterProviderBuilder, MeterProviderBuilder>? metersProviderBuilder = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder
            .ConfigureOpenTelemetry(tracerProviderBuilder, metersProviderBuilder)
            .AddDefaultHealthChecks()
            .Services
            .AddServiceDiscovery()
            .ConfigureHttpClientDefaults(http =>
            {
                // Turn on resilience by default
                http.AddStandardResilienceHandler();

                // Turn on service discovery by default
                http.AddServiceDiscovery();
            });

        return builder;
    }

    private static HostApplicationBuilder ConfigureOpenTelemetry(
        this HostApplicationBuilder builder,
        Func<TracerProviderBuilder, TracerProviderBuilder>? tracerProviderBuilder = null,
        Func<MeterProviderBuilder, MeterProviderBuilder>? metersProviderBuilder = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder
            .Logging
            .AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

        builder
            .Services
            .AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metersProviderBuilder?
                    .Invoke(metrics);
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddNpgsqlInstrumentation();
            })
            .WithTracing(tracing =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    // We want to view all traces in development
                    tracing
                        .SetSampler(new AlwaysOnSampler())
                        .AddConsoleExporter();
                }

                tracerProviderBuilder?
                    .Invoke(tracing);

                tracing
                    .AddSource(builder.Environment.ApplicationName)
                    .AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddNpgsql();
            });

        return builder
            .AddOpenTelemetryExporters();
    }

    private static HostApplicationBuilder AddOpenTelemetryExporters(this HostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

        if (useOtlpExporter)
        {
            builder
                .Services
                .AddOpenTelemetry()
                .UseOtlpExporter();
        }

        return builder;
    }

    private static HostApplicationBuilder AddDefaultHealthChecks(this HostApplicationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder
            .Services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }
}
