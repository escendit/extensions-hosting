// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

#pragma warning disable CA1812

namespace Escendit.Extensions.Hosting.Validators;

using System.Linq;
using Abstractions;
using Microsoft.Extensions.Options;

/// <summary>
/// Validates the <see cref="TemporalOptions"/> configuration settings.
/// </summary>
internal class TemporalOptionsValidator : IValidateOptions<TemporalOptions>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, TemporalOptions options)
    {
        if (!options.Grpc.Any())
        {
            return ValidateOptionsResult.Fail($"No endpoint (gRPC) provided for connection '{name}'");
        }

        if (string.IsNullOrWhiteSpace(options.Namespace))
        {
            return ValidateOptionsResult.Fail($"No namespace provided for connection '{name}'");
        }

        return string.IsNullOrWhiteSpace(options.Queue)
            ? ValidateOptionsResult.Fail($"No queue provided for connection '{name}'")
            : ValidateOptionsResult.Success;
    }
}
