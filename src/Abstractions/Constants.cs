// Licensed to the Escendit GmbH under one or more agreements.
// The Escendit GmbH licenses this file to you under the Apache License 2.0.

namespace Escendit.Extensions.Hosting.Abstractions;

#pragma warning disable CA1034

using System.Diagnostics;

/// <summary>
/// Constants.
/// </summary>
public static class Constants
{
    /// <summary>
    /// Runtime Constants.
    /// </summary>
    public static class Runtime
    {
        /// <summary>
        /// Server Constants.
        /// </summary>
        public static class Server
        {
            /// <summary>
            /// Runtime Server Source Name.
            /// </summary>
            public const string Name = "Escendit.Runtime.Server";

            /// <summary>
            /// Runtime Server Activity Source.
            /// </summary>
            public static readonly ActivitySource Source = new(Name);
        }
    }
}
