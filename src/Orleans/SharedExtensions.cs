// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Microsoft.Extensions.Hosting;

using Configuration;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

/// <summary>
/// Provides extension methods shared across the application for configuration and hosting purposes.
/// This static class contains utility methods that help enhance the functionality of
/// the Microsoft.Extensions.Hosting namespace by extending its components.
/// </summary>
internal static class SharedExtensions
{
    extension(HostApplicationBuilder builder)
    {
        internal string GetConnectionStringValue(string connectionStringName)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(connectionStringName);

            var configurationSection = builder
                .Configuration
                .GetRequiredSection("ConnectionStrings")
                .GetSection(connectionStringName);

            if (!configurationSection.Exists() || configurationSection.Value is null)
            {
                throw new InvalidOperationException($"Connection string with name {connectionStringName} is empty or invalid in configuration.");
            }

            return configurationSection.Get<string>()!;
        }
    }
}
