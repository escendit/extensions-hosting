// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using StackExchange.Redis;

/// <summary>
/// Provides extension methods and constants for working with reminder connections
/// in supported hosting environments.
/// </summary>
public static class ReminderExtensions
{
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Configures ADO.NET reminders for Orleans in the host application builder.
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string in the configuration to use for ADO.NET reminders. Defaults to "cluster".
        /// </param>
        /// <param name="connectionStringInvariant">
        /// The database provider invariant name to use. Defaults to "Npgsql".
        /// </param>
        /// <returns>
        /// The modified <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        public HostApplicationBuilder AddAdoNetReminders(
            string connectionStringName = ReminderConnectionAdoNetStringName,
            string connectionStringInvariant = ReminderInvariantName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionStringName);
            ArgumentNullException.ThrowIfNull(connectionStringInvariant);
            builder
                .UseOrleans(
                    siloBuilder => siloBuilder
                        .UseAdoNetReminderService(
                            configureOptions =>
                            {
                                var connectionString = builder.Configuration.GetConnectionString(connectionStringName)
                                                       ?? throw new InvalidOperationException($"Connection string with name {connectionStringName} is empty or invalid in configuration.");
                                configureOptions.ConnectionString = connectionString;
                                configureOptions.Invariant = connectionStringInvariant;
                            }));
            return builder;
        }

        /// <summary>
        /// Configures Redis reminders for Orleans in the host application builder.
        /// </summary>
        /// <param name="connectionStringName">
        /// The name of the connection string in the configuration to use for Redis reminders. Defaults to "connection:reminders:redis".
        /// </param>
        /// <returns>
        /// The modified <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the builder or connectionStringName is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified connection string is empty or invalid in the configuration.
        /// </exception>
        public HostApplicationBuilder AddRedisReminders(
            string connectionStringName = ReminderConnectionRedisStringName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionStringName);
            builder
                .UseOrleans(
                    siloBuilder => siloBuilder
                        .UseRedisReminderService(options =>
                        {
                            var connectionString = builder.Configuration.GetConnectionString(connectionStringName)
                                                   ?? throw new InvalidOperationException($"Connection string with name {connectionStringName} is empty or invalid in configuration.");
                            options.ConfigurationOptions = ConfigurationOptions.Parse(connectionString);
                        }));
            return builder;
        }
    }

    private const string ReminderInvariantName = "Npgsql";
    private const string ReminderConnectionAdoNetStringName = "Connection:Reminders:AdoNet";
    private const string ReminderConnectionRedisStringName = "Connection:Reminders:Redis";
}
