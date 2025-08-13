// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using System.Text.Json;
using Configuration;
using Exceptions;
using NATS.Client.Core;
using Orleans.Streaming.NATS.Hosting;

/// <summary>
/// Provides extension methods for configuring and enhancing the <see cref="HostApplicationBuilder"/> instance.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    private const string DefaultStreamName = "Platform";
    private const string StreamingConnectionStringName = "nats";

    /// <summary>
    /// Configures and adds NATS-based streaming to the application host using Orleans.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="HostApplicationBuilder"/> instance to configure.
    /// </param>
    /// <param name="name">
    /// The name of the stream to configure. Defaults to "Platform".
    /// </param>
    /// <param name="connectionStringName">
    /// The name of the connection string to use for NATS. Defaults to "nats".
    /// </param>
    /// <returns>
    /// The configured <see cref="HostApplicationBuilder"/> instance.
    /// </returns>
    /// <exception cref="ConfigurationMissingException">
    /// Thrown when the connection string is not configured.
    /// </exception>
    public static HostApplicationBuilder AddNatsStreams(
        this HostApplicationBuilder builder,
        string name = DefaultStreamName,
        string connectionStringName = StreamingConnectionStringName)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(connectionStringName);
        return builder
            .UseOrleans(
                siloBuilder =>
                {
                    var connectionString = siloBuilder
                        .Configuration
                        .GetConnectionString(connectionStringName);

                    if (string.IsNullOrEmpty(connectionString))
                    {
                        throw new ConfigurationMissingException("No connection string configured.");
                    }

                    var uri = new Uri(connectionString);
                    siloBuilder
                        .AddNatsStreams(name, options =>
                        {
                            options.StreamName = name;
                            options.JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Default);
                            options.NatsClientOptions = NatsOpts.Default with
                            {
                                Url = uri.OriginalString,
                            };
                        });
                });
    }
}
