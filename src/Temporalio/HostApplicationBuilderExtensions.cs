// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using DependencyInjection;
using Escendit.Extensions.Hosting.Temporalio;
using global::Temporalio.Extensions.Hosting;
using global::Temporalio.Extensions.OpenTelemetry;
using Temporalio.Client;

/// <summary>
/// Provides extension methods for configuring and enhancing the <see cref="HostApplicationBuilder"/>.
/// </summary>
public static class HostApplicationBuilderExtensions
{
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Adds a Temporal client to the application builder, enabling interaction with Temporal services
        /// by configuring the client using necessary settings derived from the application configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="HostApplicationBuilder"/> instance, allowing further chaining of configuration methods.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when required configuration values, such as 'TEMPORAL_GRPC' or 'TEMPORAL_NAMESPACE', are missing or invalid.
        /// </exception>
        public HostApplicationBuilder AddTemporalClient()
        {
            var clientHost = builder.Configuration.GetValue<string>("TEMPORAL_GRPC");
            var clientNamespace = builder.Configuration.GetValue<string>("TEMPORAL_NAMESPACE");

            if (clientHost is null)
            {
                throw new InvalidOperationException($"Missing required configuration value 'TEMPORAL_GRPC'.");
            }

            if (clientNamespace is null)
            {
                throw new InvalidOperationException($"Missing required configuration value 'TEMPORAL_NAMESPACE'.");
            }

            builder
                .Services
                .AddTemporalClient(clientHost, clientNamespace);
            return builder;
        }

        /// <summary>
        /// Adds a Temporal client to the application builder, enabling interaction with Temporal services
        /// by configuring the client using necessary settings derived from the application configuration.
        /// </summary>
        /// <returns>
        /// The <see cref="HostApplicationBuilder"/> instance, allowing further chaining of configuration methods.
        /// </returns>
        public HostApplicationBuilder AddTemporalClient(Action<TemporalClientConnectOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configureOptions);
            builder.Services.AddTemporalClient(configureOptions);
            return builder;
        }

        /// <summary>
        /// Adds a Temporal hosted service to the application builder, allowing the configuration
        /// of Temporal workers and client services.
        /// </summary>
        /// <param name="configureOptions">
        /// A delegate used to configure the Temporal IO builder, enabling further customization
        /// of the Temporal client and worker settings.
        /// </param>
        /// <returns>
        /// The <see cref="HostApplicationBuilder"/> instance, enabling further chaining of configuration methods.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either the application builder or the configuration delegate is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when required configuration values for Temporal, such as 'TEMPORAL_GRPC',
        /// 'TEMPORAL_NAMESPACE', 'TEMPORAL_QUEUE', or 'TEMPORAL_BUILD_ID' are missing or invalid.
        /// </exception>
        public HostApplicationBuilder AddTemporalHostedService(Action<ITemporalioBuilder> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configureOptions);

            var clientHost = builder.Configuration.GetValue<string>("TEMPORAL_GRPC");
            var clientNamespace = builder.Configuration.GetValue<string>("TEMPORAL_NAMESPACE");
            var clientQueue = builder.Configuration.GetValue<string>("TEMPORAL_QUEUE");
            var clientBuildId = builder.Configuration.GetValue<string>("TEMPORAL_BUILD_ID");

            if (clientHost is null)
            {
                throw new InvalidOperationException($"Missing required configuration value 'TEMPORAL_GRPC'.");
            }

            if (clientNamespace is null)
            {
                throw new InvalidOperationException($"Missing required configuration value 'TEMPORAL_NAMESPACE'.");
            }

            if (clientQueue is null)
            {
                throw new InvalidOperationException($"Missing required configuration value 'TEMPORAL_QUEUE'.");
            }

            if (clientBuildId is null)
            {
                throw new InvalidOperationException($"Missing required configuration value 'TEMPORAL_BUILD_ID'.");
            }

            var internalBuilder = builder
                .Services
                .AddTemporalHostedService(
                    clientHost,
                    clientNamespace,
                    clientQueue,
                    clientBuildId);

            configureOptions.Invoke(internalBuilder);
            return builder;
        }

        /// <summary>
        /// Configures and initializes the Temporal server runtime within the application builder.
        /// </summary>
        /// <param name="configureOptions">
        /// A delegate to configure the Temporal worker service options, allowing customization of worker behavior.
        /// </param>
        /// <returns>
        /// The <see cref="HostApplicationBuilder"/> instance, enabling further chaining of configuration methods.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when either the application builder or the configuration delegate is null.
        /// </exception>
        public HostApplicationBuilder AddTemporalServerRuntime(
            Action<ITemporalWorkerServiceOptionsBuilder> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(configureOptions);
            var name = builder.Environment.ApplicationName;
            return builder
                .AddServiceDefaults(tracerProviderBuilder => tracerProviderBuilder
                    .AddSource(
                        name,
                        TracingInterceptor.ClientSource.Name,
                        TracingInterceptor.WorkflowsSource.Name,
                        TracingInterceptor.ActivitiesSource.Name))
                .AddTemporalHostedService(server => server
                    .ConfigureWorkerOptions(configureOptions));
        }
    }
}
