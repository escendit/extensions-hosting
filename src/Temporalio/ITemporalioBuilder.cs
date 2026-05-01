// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Extensions.Hosting.Temporalio;

using global::Temporalio.Client;
using global::Temporalio.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

/// <summary>
/// Provides methods for configuring Temporalio services and workers.
/// </summary>
public interface ITemporalioBuilder
{
    /// <summary>
    /// Gets the name of the service.
    /// </summary>
    /// <value>The name of the service.</value>
    public string Name { get; }

    /// <summary>
    /// Gets the services.
    /// </summary>
    /// <value>The services.</value>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the client options builder.
    /// </summary>
    /// <value>The client options.</value>
    internal OptionsBuilder<TemporalClientConnectOptions> ClientOptionsBuilder { get; }

    /// <summary>
    /// Gets the worker builder.
    /// </summary>
    /// <value>The workflow builder.</value>
    internal ITemporalWorkerServiceOptionsBuilder WorkerOptionsBuilder { get; }
}
