# NuGet Package: Escendit.Extensions.Hosting

This NuGet package provides streamlined hosting extensions for .NET applications, offering simplified configuration and dependency injection setup for building robust, distributed applications. It includes opinionated defaults and convenient extension methods for integrating popular services like Temporal.io, Orleans, NATS, and ADO.NET—with configuration-driven setup and production-ready patterns.

Key features:

- Simplified Temporal client and worker configuration with hosting integration
- Orleans clustering, persistence, and streaming setup
- NATS messaging integration
- ADO.NET connection management with resilience patterns
- Secret management integration
- Built-in dependency injection integration
- Configuration-driven service setup
- Production-ready defaults for common use cases

## Installation
To install this package, use the NuGet Package Manager Console:
```textmate
PM> Install-Package Escendit.Extensions.Hosting
```

Or search for "Escendit.Extensions.Hosting" in the NuGet Package Manager UI.


### Default Settings

The package uses the following sensible defaults (overridable via code or configuration):

- Temporal:
    - ClientTargetHost: localhost:7233
    - ClientNamespace: default
    - Worker versioning: disabled by default (enabled if buildId provided)
    - Tracing: enabled (OpenTelemetry interceptors)
- Orleans:
    - ClusterId/ServiceId required; in dev, defaults may be provided
    - Local clustering for development when not otherwise configured
- NATS:
    - Url: nats://localhost:4222
    - Reconnect and ping intervals with safe defaults
- ADO.NET:
    - Connection factory with retries, jitter, and timeout defaults
- Secrets:
    - Reads from configuration providers; supports layered sources

## Dependencies

This package integrates with and composes common .NET hosting libraries, such as:

- Temporal hosting and OpenTelemetry integrations
- Orleans runtime and hosting
- NATS .NET client
- ADO.NET providers and Microsoft.Extensions.* packages

These are pulled transitively as needed when you install this package.

## Contributing
If you find a bug or have a feature request, please create an issue in the repository.

To contribute code, fork the repository and submit a pull request.
Please ensure that your changes follow the project's coding standards and include appropriate tests.

## License
Licensed under Apache License 2.0. See the LICENSE file for the complete license text.
