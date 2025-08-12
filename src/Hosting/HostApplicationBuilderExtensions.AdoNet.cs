// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;

/// <summary>
/// Provides extension methods for enhancing the functionality of <see cref="Microsoft.Extensions.Hosting.HostApplicationBuilder"/>.
/// This class can include methods that assist in configuring or setting up services, middleware, or infrastructure components
/// during the application host building process.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    private const string DefaultInvariant = "Npgsql";
    private const string ClusterConnectionStringName = "cluster";

    /// <summary>
    /// Configures ADO.NET clustering for Orleans in the host application builder.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="HostApplicationBuilder"/> instance to configure.
    /// </param>
    /// <param name="connectionStringName">
    /// The name of the connection string in the configuration to use for ADO.NET clustering. Defaults to "cluster".
    /// </param>
    /// <param name="connectionStringInvariant">
    /// The database provider invariant name to use. Defaults to "Npgsql".
    /// </param>
    /// <returns>
    /// The modified <see cref="HostApplicationBuilder"/> instance.
    /// </returns>
    public static HostApplicationBuilder AddAdoNetClustering(
        this HostApplicationBuilder builder,
        string connectionStringName = ClusterConnectionStringName,
        string connectionStringInvariant = DefaultInvariant)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(connectionStringName);
        ArgumentNullException.ThrowIfNull(connectionStringInvariant);
        return builder
            .UseOrleans(
                siloBuilder => siloBuilder
                    .UseAdoNetClustering(
                        configureOptions =>
                        {
                            configureOptions.ConnectionString = siloBuilder.Configuration.GetConnectionString(connectionStringName);
                            configureOptions.Invariant = connectionStringInvariant;
                        }));
    }

    /// <summary>
    /// Configures ADO.NET reminders for Orleans in the host application builder.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="HostApplicationBuilder"/> instance to configure.
    /// </param>
    /// <param name="connectionStringName">
    /// The name of the connection string in the configuration to use for ADO.NET reminders. Defaults to "cluster".
    /// </param>
    /// <param name="connectionStringInvariant">
    /// The database provider invariant name to use. Defaults to "Npgsql".
    /// </param>
    /// <returns>
    /// The modified <see cref="HostApplicationBuilder"/> instance.
    /// </returns>
    public static HostApplicationBuilder AddAdoNetReminders(
        this HostApplicationBuilder builder,
        string connectionStringName = ClusterConnectionStringName,
        string connectionStringInvariant = DefaultInvariant)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(connectionStringName);
        ArgumentNullException.ThrowIfNull(connectionStringInvariant);
        return builder
            .UseOrleans(
                siloBuilder => siloBuilder
                    .UseAdoNetReminderService(
                        configureOptions =>
                        {
                            configureOptions.ConnectionString = siloBuilder.Configuration.GetConnectionString(connectionStringName);
                            configureOptions.Invariant = connectionStringInvariant;
                        }));
    }

    /// <summary>
    /// Configures ADO.NET-based persistence as the default grain storage provider for Orleans in the host application builder.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="HostApplicationBuilder"/> instance to configure.
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
    public static HostApplicationBuilder AddAdoNetPersistenceAsDefault(
        this HostApplicationBuilder builder,
        string connectionStringName = ClusterConnectionStringName,
        string connectionStringInvariant = DefaultInvariant)
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
    /// <param name="builder">
    /// The <see cref="HostApplicationBuilder"/> instance to configure.
    /// </param>
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
    public static HostApplicationBuilder AddAdoNetPersistence(
        this HostApplicationBuilder builder,
        string name,
        string connectionStringName = ClusterConnectionStringName,
        string connectionStringInvariant = DefaultInvariant)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);

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
