// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using System.Text.Json;
using DependencyInjection;
using Escendit.Extensions.Hosting.Temporalio;
using global::Temporalio.Extensions.Hosting;
using global::Temporalio.Extensions.OpenTelemetry;
using Temporalio.Common;
using Temporalio.Converters;
using Temporalio.Worker;

/// <summary>
/// Provides extension methods for configuring Temporalio services and workers
/// in an <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Provides extension methods for configuring Temporalio-related services
    /// in an <see cref="IServiceCollection"/>.
    /// </summary>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// Adds a Temporal hosted service to the service collection.
        /// </summary>
        /// <param name="clientTargetHost">The target host for the Temporal client.</param>
        /// <param name="clientNamespace">The namespace to be used by the Temporal client.</param>
        /// <param name="queueName">The task queue name used by the Temporal worker.</param>
        /// <param name="buildId">
        /// Optional. The build ID used for worker versioning. If not provided, no versioning configuration is applied.
        /// </param>
        /// <param name="disallowDuplicates">
        /// Optional. Specifies whether duplicate tasks are disallowed. Defaults to <c>false</c>.
        /// </param>
        /// <returns>An instance of <see cref="ITemporalioBuilder"/> configured with the Temporal client and worker options.</returns>
        public ITemporalioBuilder AddTemporalHostedService(
            string clientTargetHost,
            string clientNamespace,
            string queueName,
            string? buildId = null,
            bool disallowDuplicates = false)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(clientTargetHost);
            ArgumentNullException.ThrowIfNull(clientNamespace);
            ArgumentNullException.ThrowIfNull(queueName);
            var name = string.Empty;
            var clientOptionsBuilder = services
                .AddTemporalClient(clientTargetHost, clientNamespace)
                .Configure(options =>
                {
                    var serializerOptions = new JsonSerializerOptions();
                    options.DataConverter = new DataConverter(new DefaultPayloadConverter(serializerOptions), new DefaultFailureConverter());
                    options.Interceptors = [new TracingInterceptor()];
                });

            var deploymentOptions = buildId is null
                ? null
                : new WorkerDeploymentOptions
                {
                    UseWorkerVersioning = true,
                    DefaultVersioningBehavior = VersioningBehavior.Unspecified,
                    Version = new WorkerDeploymentVersion(name, buildId),
                };

            var workerOptionsBuilder = services
                .AddHostedTemporalWorker(
                    clientTargetHost,
                    clientNamespace,
                    queueName,
                    deploymentOptions)
                .ConfigureOptions(
                    options =>
                    {
                        var serializerOptions = new JsonSerializerOptions();

                        options.Interceptors = [new TracingInterceptor()];

                        if (options.ClientOptions is null)
                        {
                            return;
                        }

                        options.ClientOptions.DataConverter = new DataConverter(new DefaultPayloadConverter(serializerOptions), new DefaultFailureConverter());
                        options.ClientOptions.Interceptors = [new TracingInterceptor()];
                    },
                    disallowDuplicates);

            return new TemporalioBuilder(name, services, clientOptionsBuilder, workerOptionsBuilder);
        }
    }
}
