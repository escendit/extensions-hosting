// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Extensions.Hosting.Temporalio;

using global::Temporalio.Client;
using global::Temporalio.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// Provides methods for building and configuring integrations with Temporalio.
/// </summary>
internal sealed class TemporalioBuilder : ITemporalioBuilder
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemporalioBuilder"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="serviceCollection">The service collection.</param>
    /// <param name="clientOptionsBuilder">The client options builder.</param>
    /// <param name="workerOptionsBuilder">The worker options builder.</param>
    public TemporalioBuilder(
        string name,
        IServiceCollection serviceCollection,
        OptionsBuilder<TemporalClientConnectOptions> clientOptionsBuilder,
        ITemporalWorkerServiceOptionsBuilder workerOptionsBuilder)
    {
        Name = name;
        Services = serviceCollection;
        ClientOptionsBuilder = clientOptionsBuilder;
        WorkerOptionsBuilder = workerOptionsBuilder;
    }

    /// <inheritdoc/>
    public string Name { get; }

    /// <inheritdoc/>
    public IServiceCollection Services { get; }

    /// <inheritdoc/>
    public OptionsBuilder<TemporalClientConnectOptions> ClientOptionsBuilder { get; }

    /// <inheritdoc/>
    public ITemporalWorkerServiceOptionsBuilder WorkerOptionsBuilder { get; }
}
