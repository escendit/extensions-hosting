// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Extensions.Hosting.Temporalio;

using global::Temporalio.Client;
using global::Temporalio.Extensions.Hosting;
using Microsoft.Extensions.Options;

/// <summary>
/// Defines a set of static methods for extending the functionality of a Temporal.io builder.
/// </summary>
public static class TemporalioBuilderExtensions
{
    /// <summary>
    /// Provides extension methods for configuring an <see cref="ITemporalioBuilder"/> instance.
    /// </summary>
    extension(ITemporalioBuilder temporalBuilder)
    {
        /// <summary>
        /// Configure Client Options.
        /// </summary>
        /// <param name="configureOptions">The configure client options.</param>
        /// <returns>The updated temporal migrator builder.</returns>
        public ITemporalioBuilder ConfigureClientOptions(
            Action<OptionsBuilder<TemporalClientConnectOptions>> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(temporalBuilder);
            ArgumentNullException.ThrowIfNull(configureOptions);
            configureOptions.Invoke(temporalBuilder.ClientOptionsBuilder);
            return temporalBuilder;
        }

        /// <summary>
        /// Configure Client Options.
        /// </summary>
        /// <param name="configureOptions">The configure client options.</param>
        /// <returns>The updated temporal migrator builder.</returns>
        public ITemporalioBuilder ConfigureClientOptions(
            Action<TemporalClientConnectOptions> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(temporalBuilder);
            ArgumentNullException.ThrowIfNull(configureOptions);
            return ConfigureClientOptions(temporalBuilder, builder => builder.Configure(configureOptions));
        }

        /// <summary>
        /// Configure Worker Options.
        /// </summary>
        /// <param name="configureOptions">The configure options.</param>
        /// <returns>The updated temporal migrator builder.</returns>
        public ITemporalioBuilder ConfigureWorkerOptions(
            Action<ITemporalWorkerServiceOptionsBuilder> configureOptions)
        {
            ArgumentNullException.ThrowIfNull(temporalBuilder);
            ArgumentNullException.ThrowIfNull(configureOptions);
            configureOptions.Invoke(temporalBuilder.WorkerOptionsBuilder);
            return temporalBuilder;
        }
    }
}
