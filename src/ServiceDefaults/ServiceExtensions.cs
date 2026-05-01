// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using DependencyInjection;
using Diagnostics.HealthChecks;
using Http.Resilience;
using Logging;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Options;
using Polly;
using OpenTelemetryBuilder = OpenTelemetry.OpenTelemetryBuilder;

/// <summary>
/// Provides extension methods for configuring services in the <see cref="Microsoft.Extensions.Hosting"/> namespace.
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Provides extension methods for configuring default services, including OpenTelemetry instrumentation, health checks,
    /// HTTP client configurations, service discovery, and resilience mechanisms, in the <see cref="Microsoft.Extensions.Hosting"/> namespace.
    /// </summary>
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Configures the provided <see cref="HostApplicationBuilder"/> with default service settings.
        /// This includes logging, OpenTelemetry instrumentation, health checks, service discovery,
        /// HTTP client configurations, and resilience mechanisms.
        /// </summary>
        /// <param name="tracerProviderBuilder">
        /// Optional action to customize the <see cref="TracerProviderBuilder"/> for tracing configurations.
        /// If not provided, default tracing settings will be applied.
        /// </param>
        /// <param name="metersProviderBuilder">
        /// Optional action to customize the <see cref="MeterProviderBuilder"/> for metrics configurations.
        /// If not provided, default metric settings will be applied.
        /// </param>
        /// <param name="httpClientBuilder">
        /// Optional action to configure the <see cref="IHttpClientBuilder"/> for HTTP client behaviors.
        /// If not provided, HTTP client settings are skipped.
        /// </param>
        /// <returns>The configured <see cref="HostApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <see cref="HostApplicationBuilder"/> instance is null.
        /// </exception>
        public HostApplicationBuilder AddServiceDefaults(
            Action<TracerProviderBuilder>? tracerProviderBuilder = null,
            Action<MeterProviderBuilder>? metersProviderBuilder = null,
            Action<IHttpClientBuilder>? httpClientBuilder = null)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder
                .AddLogging()
                .AddOpenTelemetry(telemetryBuilder =>
                {
                    telemetryBuilder
                        .WithDefaultMetrics(metrics =>
                        {
                            metersProviderBuilder?.Invoke(metrics);
                        })
                        .WithDefaultTracing(builder.Environment, tracing =>
                        {
                            tracerProviderBuilder?.Invoke(tracing);
                        });
                })
                .AddDefaultHealthChecks()
                .AddServiceDiscovery(httpClientBuilder)
                .AddOpenTelemetryExporters();

            return builder;
        }

        /// <summary>
        /// Configures the provided <see cref="HostApplicationBuilder"/> with default service settings optimized for low latency.
        /// This includes OpenTelemetry instrumentation, health checks, HTTP client configurations with service discovery,
        /// and the addition of a standard resilience handler for HTTP calls.
        /// </summary>
        /// <param name="tracerProviderBuilder">
        /// Optional action to customize the <see cref="TracerProviderBuilder"/> for tracing configurations.
        /// If not provided, default tracing settings will be applied.
        /// </param>
        /// <param name="metersProviderBuilder">
        /// Optional action to customize the <see cref="MeterProviderBuilder"/> for metrics configurations.
        /// If not provided, default metric settings will be applied.
        /// </param>
        /// <returns>The configured <see cref="HostApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <see cref="HostApplicationBuilder"/> instance is null.
        /// </exception>
        public HostApplicationBuilder AddLowLatencyServiceDefaults(
            Action<TracerProviderBuilder>? tracerProviderBuilder = null,
            Action<MeterProviderBuilder>? metersProviderBuilder = null)
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder
                .AddServiceDefaults(tracerProviderBuilder, metersProviderBuilder, httpClientBuilder =>
                {
                    httpClientBuilder
                        .AddServiceDiscovery()
                        .AddStandardResilienceHandler();
                });
        }

        /// <summary>
        /// Configures the provided <see cref="HostApplicationBuilder"/> with default settings tailored for backend services.
        /// This includes logging, OpenTelemetry instrumentation, health checks, service discovery, HTTP client configurations.
        /// </summary>
        /// <param name="tracerProviderBuilder">
        /// Optional action to customize the <see cref="TracerProviderBuilder"/> for tracing configurations.
        /// If not provided, default tracing settings are applied for backend services.
        /// </param>
        /// <param name="metersProviderBuilder">
        /// Optional action to customize the <see cref="MeterProviderBuilder"/> for metrics configurations.
        /// If not provided, default metric settings are applied for backend services.
        /// </param>
        /// <returns>The configured <see cref="HostApplicationBuilder"/> instance.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <see cref="HostApplicationBuilder"/> instance is null.
        /// </exception>
        public HostApplicationBuilder AddBackendServiceDefaults(
            Action<TracerProviderBuilder>? tracerProviderBuilder = null,
            Action<MeterProviderBuilder>? metersProviderBuilder = null)
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder
                .AddServiceDefaults(tracerProviderBuilder, metersProviderBuilder, httpClientBuilder =>
                {
                    httpClientBuilder
                        .AddServiceDiscovery();
                });
        }

        private HostApplicationBuilder AddOpenTelemetryExporters()
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

        private HostApplicationBuilder AddDefaultHealthChecks()
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder
                .Services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

            return builder;
        }

        private HostApplicationBuilder AddLogging()
        {
            builder
                .Logging
                .AddOpenTelemetry(logging =>
                {
                    logging.IncludeFormattedMessage = true;
                    logging.IncludeScopes = true;
                });
            return builder;
        }

        private HostApplicationBuilder AddOpenTelemetry(Action<OpenTelemetryBuilder> configure)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configure);
            configure(builder.Services.AddOpenTelemetry());
            return builder;
        }

        private HostApplicationBuilder AddServiceDiscovery(Action<IHttpClientBuilder>? configure)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder
                .Services
                .AddServiceDiscovery();

            if (configure is null)
            {
                return builder;
            }

            builder
                .Services
                .ConfigureHttpClientDefaults(configure);

            return builder;
        }
    }
}
