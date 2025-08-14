// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Extensions.Hosting.Validators;

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
        if (options.Grpc is null || !options.Grpc.Any())
        {
            return ValidateOptionsResult.Fail($"No hostname provided for connection '{name}'");
        }

        if (options.Namespace is null)
        {
            return ValidateOptionsResult.Fail($"No namespace provided for connection '{name}'");
        }

        return options.Queue is null
            ? ValidateOptionsResult.Fail($"No queue provided for connection '{name}'")
            : ValidateOptionsResult.Success;
    }
}
