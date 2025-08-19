# NuGet Package: Escendit.Extensions.Abstractions

This NuGet package provides shared interfaces, options, and dependency-injection helpers used across Escendit hosting extensions for .NET applications. It offers consistent primitives for configuration, service registration, keyed services, and telemetry/health abstractions—so feature-specific packages (e.g., workflows, messaging, storage) can plug in with the same developer experience.

## Installation
To install this package, use the NuGet Package Manager Console:
```textmate
PM> Install-Package Escendit.Extensions.Abstractions
```

Or search for "Escendit.Extensions.Abstractions" in the NuGet Package Manager UI.

## Contributing
Issues and pull requests are welcome. Please follow repository guidelines, include tests where applicable, and keep abstractions minimal and dependency-light.

## License
Licensed under the Apache License 2.0. See the LICENSE file for details.
