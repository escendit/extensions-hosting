// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using System.ComponentModel.DataAnnotations;
using Configuration;

/// <summary>
/// Provides extension methods for configuring clustering options
/// in a Microsoft.Extensions.Hosting environment.
/// </summary>
public static class ClusteringExtensions
{
    /// <summary>
    /// Contains extension methods for configuring clustering features
    /// within the Orleans hosting environment.
    /// </summary>
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Configures ADO.NET server clustering for Orleans in the host application builder.
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string in the configuration to use for ADO.NET clustering. Defaults to "connection:cluster:adonet".
        /// </param>
        /// <param name="connectionStringInvariant">
        /// The invariant name of the database provider to use for ADO.NET clustering. Defaults to "Npgsql".
        /// </param>
        /// <returns>
        /// The modified <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="connectionStringName"/> or <paramref name="connectionStringInvariant"/> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the connection string with the specified <paramref name="connectionStringName"/> cannot be found in the configuration.
        /// </exception>
        public HostApplicationBuilder AddAdoNetServerClustering(
            string connectionStringName = ClusterConnectionAdoNetStringName,
            string connectionStringInvariant = ClusterInvariantName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionStringName);
            ArgumentNullException.ThrowIfNull(connectionStringInvariant);
            builder
                .UseOrleans(
                    siloBuilder => siloBuilder
                        .UseAdoNetClustering(
                            configureOptions =>
                            {
                                configureOptions.ConnectionString = builder.GetConnectionStringValue(connectionStringName);
                                configureOptions.Invariant = connectionStringInvariant;
                            }));
            return builder;
        }

        /// <summary>
        /// Configures ADO.NET client clustering for Orleans in the host application builder.
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string in the configuration to use for ADO.NET clustering. Defaults to "connection:cluster:adonet".
        /// </param>
        /// <param name="connectionStringInvariant">
        /// The invariant name of the database provider to use for ADO.NET clustering. Defaults to "Npgsql".
        /// </param>
        /// <returns>
        /// The modified <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="connectionStringName"/> or <paramref name="connectionStringInvariant"/> is null.
        /// </exception>
        public HostApplicationBuilder AddAdoNetClientClustering(
            string connectionStringName = ClusterConnectionAdoNetStringName,
            string connectionStringInvariant = ClusterInvariantName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionStringName);
            ArgumentNullException.ThrowIfNull(connectionStringInvariant);

            builder
                .UseOrleansClient(clientBuilder => clientBuilder
                    .UseAdoNetClustering(configureOptions =>
                    {
                        configureOptions.ConnectionString = builder.GetConnectionStringValue(connectionStringName);
                        configureOptions.Invariant = connectionStringInvariant;
                    }));
            return builder;
        }

        /// <summary>
        /// Configures Redis server clustering for Orleans in the host application builder.
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string in the configuration to use for Redis clustering. Defaults to "connection:cluster:redis".
        /// </param>
        /// <returns>
        /// The modified <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="connectionStringName"/> is null.
        /// </exception>
        public HostApplicationBuilder AddRedisServerClustering(
            string connectionStringName = ClusterConnectionRedisStringName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionStringName);

            builder
                .UseOrleans(
                    siloBuilder => siloBuilder
                        .UseRedisClustering(builder.GetConnectionStringValue(connectionStringName)));
            return builder;
        }

        /// <summary>
        /// Configures Redis clustering for Orleans in the host application builder.
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string in the configuration to use for Redis clustering. Defaults to "redis".
        /// </param>
        /// <returns>
        /// The modified <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the builder or connection string name is null.
        /// </exception>
        /// <exception cref="ValidationException">
        /// Thrown when the specified connection string is not found in the configuration.
        /// </exception>
        public HostApplicationBuilder AddRedisClientClustering(
            string connectionStringName = ClusterConnectionRedisStringName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionStringName);

            builder
                .UseOrleansClient(clientBuilder => clientBuilder
                    .UseRedisClustering(builder.GetConnectionStringValue(connectionStringName)));
            return builder;
        }
    }

    private const string ClusterInvariantName = "Npgsql";
    private const string ClusterConnectionAdoNetStringName = "Connection:Cluster:AdoNet";
    private const string ClusterConnectionRedisStringName = "Connection:Cluster:Redis";
}
