// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using DependencyInjection;
using Orleans.Configuration;

/// <summary>
/// Provides extension methods for configuring and managing
/// Orleans client functionalities within the application host.
/// </summary>
public static class ClientExtensions
{
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Configures the Orleans client runtime within the specified application builder.
        /// </summary>
        /// <returns>
        /// The <see cref="HostApplicationBuilder"/> instance, enabling method chaining.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the provided <see cref="HostApplicationBuilder"/> is null.
        /// </exception>
        public HostApplicationBuilder AddOrleansClientRuntime()
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder
                .AddClientClusteringDefaults()
                .AddClientActivityPropagation();
            return builder;
        }

        /// <summary>
        /// Configures the Orleans client runtime within the specified application builder.
        /// </summary>
        /// <returns>
        /// The <see cref="HostApplicationBuilder"/> instance, enabling method chaining.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the provided <see cref="HostApplicationBuilder"/> is null.
        /// </exception>
        public HostApplicationBuilder AddOrleansClientRuntime(Action<IClientBuilder> configureAction)
        {
            ArgumentNullException.ThrowIfNull(configureAction);
            builder.UseOrleansClient(configureAction);
            return builder;
        }

        private HostApplicationBuilder AddClientClusteringDefaults()
        {
            ArgumentNullException.ThrowIfNull(builder);

            return builder
                .UseOrleansClient(clientBuilder => clientBuilder
                    .ConfigureServices(services => services
                        .Configure<ClusterOptions>(configureOptions => builder
                            .Configuration
                            .Bind(configureOptions))));
        }

        private HostApplicationBuilder AddClientActivityPropagation()
        {
            return builder
                .UseOrleansClient(clientBuilder => clientBuilder
                    .AddActivityPropagation());
        }
    }
}
