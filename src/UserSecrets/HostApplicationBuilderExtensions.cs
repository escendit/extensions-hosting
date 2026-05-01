// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;

/// <summary>
/// Provides extension methods for configuring and customizing the <see cref="HostApplicationBuilder"/>.
/// This class is a static helper designed to extend functionality for building and configuring
/// host applications in .NET, offering streamlined configuration options and utility methods for
/// working with the <see cref="HostApplicationBuilder"/>.
/// </summary>
public static class HostApplicationBuilderExtensions
{
    /// <summary>
    /// Provides extension methods for configuring and customizing the <see cref="HostApplicationBuilder"/>.
    /// This static class includes utility methods to augment the functionality of the host application setup,
    /// offering streamlined mechanisms for working with application configuration and environment-specific
    /// settings.
    /// </summary>
    extension(HostApplicationBuilder builder)
    {
        /// <summary>
        /// Adds user secrets configuration to the application configuration for the specified type.
        /// </summary>
        /// <typeparam name="TType">
        /// The type used to determine the assembly from which to load the user secrets.
        /// </typeparam>
        /// <returns>
        /// The <see cref="HostApplicationBuilder"/> with user secrets added to its configuration.
        /// </returns>
        public HostApplicationBuilder AddUserSecrets<TType>()
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
}
