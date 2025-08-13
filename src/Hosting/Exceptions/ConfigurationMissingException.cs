// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Extensions.Hosting.Exceptions;

using System;

/// <summary>
/// Configuration Missing Exception.
/// </summary>
[Serializable]
public sealed class ConfigurationMissingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public ConfigurationMissingException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
    /// </summary>
    public ConfigurationMissingException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationMissingException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="innerException">The inner exception.</param>
    public ConfigurationMissingException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
