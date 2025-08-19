// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;

/// <summary>
/// Provides extension methods for the <see cref="HostApplicationBuilder"/> class
/// to enhance and customize the application hosting behavior.
/// </summary>
public static partial class HostApplicationBuilderExtensions
{
    /// <summary>
    /// Adds user secrets configuration to the application configuration for the specified type.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="HostApplicationBuilder"/> to add user secrets to.
    /// </param>
    /// <typeparam name="TType">
    /// The type used to determine the assembly from which to load the user secrets.
    /// </typeparam>
    /// <returns>
    /// The <see cref="HostApplicationBuilder"/> with user secrets added to its configuration.
    /// </returns>
    public static HostApplicationBuilder AddUserSecrets<TType>(this HostApplicationBuilder builder)
        where TType : class
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.Environment.IsDevelopment())
        {
            builder
                .Configuration
                .AddUserSecrets<TType>();
        }

        return builder;
    }
}
