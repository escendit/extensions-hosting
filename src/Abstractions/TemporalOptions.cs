// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Extensions.Hosting.Abstractions;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Provides configuration options for Temporal settings.
/// </summary>
[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)]
public record TemporalOptions
{
    /// <summary>
    /// Gets or sets the grpc connection strings.
    /// </summary>
    /// <value>The grpc connections.</value>
    public IEnumerable<Uri> Grpc { get; set; } = [];

    /// <summary>
    /// Gets or sets the queue name.
    /// </summary>
    /// <value>The queue name.</value>
    public string Queue { get; set; } = null!;

    /// <summary>
    /// Gets or sets the namespace.
    /// </summary>
    /// <value>The namespace.</value>
    public string Namespace { get; set; } = null!;

    /// <summary>
    /// Gets or sets the build id.
    /// </summary>
    /// <value>The build id.</value>
    public string? BuildId { get; set; }
}
