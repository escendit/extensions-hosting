// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;

/// <summary>
/// Provides extension methods for configuring storage services in a host application builder.
/// </summary>
public static class StorageExtensions
{
    /// <summary>
    /// Contains extension methods for configuring ADO.NET-based persistence and grain storage for Orleans in a host application builder.
    /// </summary>
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Configures ADO.NET-based persistence as the default grain storage provider for Orleans in the host application builder.
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string in the configuration to use for ADO.NET persistence. Defaults to "cluster".
        /// </param>
        /// <param name="connectionStringInvariant">
        /// The database provider invariant name to use. Defaults to "Npgsql".
        /// </param>
        /// <returns>
        /// The modified <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        public HostApplicationBuilder AddAdoNetStorageAsDefault(
            string connectionStringName = StorageConnectionAdoNetStringName,
            string connectionStringInvariant = StorageInvariantName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionStringName);
            ArgumentNullException.ThrowIfNull(connectionStringInvariant);
            return builder
                .UseOrleans(
                    siloBuilder => siloBuilder
                        .AddAdoNetGrainStorageAsDefault(
                            configureOptions =>
                            {
                                configureOptions.ConnectionString = siloBuilder.Configuration.GetConnectionString(connectionStringName);
                                configureOptions.Invariant = connectionStringInvariant;
                            }));
        }

        /// <summary>
        /// Configures ADO.NET-based persistence for Orleans by adding a specified storage provider.
        /// </summary>
        /// <param name="name">
        /// The name of the storage provider.
        /// </param>
        /// <param name="connectionStringName">
        /// The name of the connection string in the configuration to use for ADO.NET persistence. Defaults to "cluster".
        /// </param>
        /// <param name="connectionStringInvariant">
        /// The database provider invariant name to use. Defaults to "Npgsql".
        /// </param>
        /// <returns>
        /// The modified <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        public HostApplicationBuilder AddAdoNetStorage(
            string name,
            string connectionStringName = StorageConnectionAdoNetStringName,
            string connectionStringInvariant = StorageInvariantName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(connectionStringName);
            ArgumentNullException.ThrowIfNull(connectionStringInvariant);

            builder
                .UseOrleans(
                    siloBuilder => siloBuilder
                        .AddAdoNetGrainStorage(
                            name,
                            configureOptions =>
                            {
                                configureOptions.ConnectionString = builder
                                    .Configuration
                                    .GetConnectionString(connectionStringName);
                                configureOptions.Invariant = connectionStringInvariant;
                            }));

            return builder;
        }
    }

    private const string StorageInvariantName = "Npgsql";
    private const string StorageConnectionAdoNetStringName = "connection:storage:adonet";
}
