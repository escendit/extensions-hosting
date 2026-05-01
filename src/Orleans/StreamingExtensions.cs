// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using System.Text.Json;
using Configuration;
using NATS.Client.Core;
using Orleans.Streaming.NATS.Hosting;

/// <summary>
/// Provides extension methods for configuring streaming capabilities in an application host,
/// specifically for integrating NATS-based streaming with Orleans using Microsoft.Extensions.Hosting.
/// </summary>
public static class StreamingExtensions
{
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Configures and adds NATS server-based streaming to the application host using Orleans.
        /// </summary>
        /// <param name="name">
        /// The name of the stream to configure. Defaults to "Platform".
        /// </param>
        /// <param name="connectionStringName">
        /// The name of the connection string to use for NATS. Defaults to "nats".
        /// </param>
        /// <returns>
        /// The configured <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the builder, name, or connection string name is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the connection string is either not configured or in an invalid format.
        /// </exception>
        public HostApplicationBuilder AddNatsServerStreams(
            string name = DefaultStreamName,
            string connectionStringName = StreamingConnectionNatsStringName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(connectionStringName);
            builder
                .UseOrleans(siloBuilder =>
                {
                    var connectionString = siloBuilder
                        .Configuration
                        .GetConnectionString(connectionStringName);

                    try
                    {
                        if (string.IsNullOrEmpty(connectionString))
                        {
                            throw new InvalidOperationException($"The connection string with name '{connectionStringName}' is not configured.");
                        }

                        var uri = new Uri(connectionString);
                        siloBuilder
                            .AddNatsStreams(name, options =>
                            {
                                options.StreamName = name;
                                options.JsonSerializerOptions =
                                    new JsonSerializerOptions(JsonSerializerOptions.Default);
                                options.NatsClientOptions = NatsOpts.Default with
                                {
                                    Url = uri.OriginalString,
                                };
                            });
                    }
                    catch (UriFormatException)
                    {
                        throw new InvalidOperationException($"The connection string with name '{connectionStringName}' is in invalid format.");
                    }
                });
            return builder;
        }

        /// <summary>
        /// Configures and adds NATS client-based streaming to the application host using Orleans.
        /// </summary>
        /// <param name="name">
        /// The name of the client stream to configure. Defaults to "Platform".
        /// </param>
        /// <param name="connectionStringName">
        /// The name of the connection string to use for NATS. Defaults to "nats".
        /// </param>
        /// <returns>
        /// The configured <see cref="HostApplicationBuilder"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the builder, name, or connection string name is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the connection string is either not configured or in an invalid format.
        /// </exception>
        public HostApplicationBuilder AddNatsClientStreams(
            string name = DefaultStreamName,
            string connectionStringName = StreamingConnectionNatsStringName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(name);
            ArgumentNullException.ThrowIfNull(connectionStringName);
            builder
                .UseOrleansClient(clientBuilder =>
                {
                    var connectionString = clientBuilder
                        .Configuration
                        .GetConnectionString(connectionStringName);

                    try
                    {
                        if (string.IsNullOrEmpty(connectionString))
                        {
                            throw new InvalidOperationException(
                                $"The connection string with name '{connectionStringName}' is not configured.");
                        }

                        var uri = new Uri(connectionString);
                        clientBuilder
                            .AddNatsStreams(name, options =>
                            {
                                options.StreamName = name;
                                options.JsonSerializerOptions =
                                    new JsonSerializerOptions(JsonSerializerOptions.Default);
                                options.NatsClientOptions = NatsOpts.Default with
                                {
                                    Url = uri.OriginalString,
                                };
                            });
                    }
                    catch (UriFormatException)
                    {
                        throw new InvalidOperationException(
                            $"The connection string with name '{connectionStringName}' is in invalid format.");
                    }
                });
            return builder;
        }
    }

    private const string DefaultStreamName = "Platform";
    private const string StreamingConnectionNatsStringName = "nats";
}
