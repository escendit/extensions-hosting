// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Npgsql;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

/// <summary>
/// Provides extension methods for integrating OpenTelemetry with the hosting environment.
/// </summary>
public static class OpenTelemetryExtensions
{
    /// <summary>
    /// Adds custom configurations and extensions to the provided <see cref="OpenTelemetryBuilder"/>.
    /// </summary>
    extension(OpenTelemetryBuilder builder)
    {
        /// <summary>
        /// Configures the provided <see cref="OpenTelemetryBuilder"/> with default metric settings.
        /// This includes predefined meter configurations and extensions for telemetry collection.
        /// </summary>
        /// <param name="providerBuilder">
        /// Optional function to customize the <see cref="MeterProviderBuilder"/> configuration.
        /// If null, default settings will be applied without customization.
        /// </param>
        /// <returns>The configured <see cref="OpenTelemetryBuilder"/> instance.</returns>
        public OpenTelemetryBuilder WithDefaultMetrics(Action<MeterProviderBuilder>? providerBuilder = null)
        {
            builder
                .WithMetrics(metrics =>
                {
                    providerBuilder?.Invoke(metrics);

                    metrics
                        .AddHttpClientInstrumentation()
                        .AddProcessInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddNpgsqlInstrumentation();
                });
            return builder;
        }

        /// <summary>
        /// Configures the provided <see cref="OpenTelemetryBuilder"/> with default tracing settings.
        /// This includes predefined sampler, instrumentation, and trace source configurations
        /// tailored for the environment and application.
        /// In development environments, traces will utilize a console exporter and an
        /// always-on sampler for comprehensive debugging.
        /// </summary>
        /// <param name="environment">
        /// The <see cref="IHostEnvironment"/> containing the runtime environment information,
        /// such as development, staging, or production.
        /// </param>
        /// <param name="providerBuilder">
        /// Optional function to customize the <see cref="TracerProviderBuilder"/> configuration.
        /// If null, default tracing settings will be applied without customization.
        /// </param>
        /// <returns>The configured <see cref="OpenTelemetryBuilder"/> instance.</returns>
        public OpenTelemetryBuilder WithDefaultTracing(
            IHostEnvironment environment,
            Action<TracerProviderBuilder>? providerBuilder = null)
        {
            builder
                .WithTracing(traces =>
                {
                    if (environment.IsDevelopment())
                    {
                        // We want to view all traces in development
                        traces
                            .SetSampler(new AlwaysOnSampler())
                            .AddConsoleExporter();
                    }

                    providerBuilder?.Invoke(traces);

                    traces
                        .AddSource(environment.ApplicationName)
                        .AddGrpcClientInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddNpgsql();
                });
            return builder;
        }
    }
}
