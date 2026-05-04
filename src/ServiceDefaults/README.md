# Service Defaults Hosting Extensions

[![NuGet Version](https://img.shields.io/nuget/v/Escendit.Extensions.Hosting.ServiceDefaults.svg)](https://www.nuget.org/packages/Escendit.Extensions.Hosting.ServiceDefaults)

Opinionated service defaults for .NET applications, inspired by .NET Aspire but tailored for standalone hosting.

## Features

- **OpenTelemetry:** Configures Metrics, Tracing, and Logging with OTLP exporter support.
- **Health Checks:** Adds a default "self" health check.
- **Service Discovery:** Integrates Microsoft.Extensions.ServiceDiscovery.
- **Resilience:** Provides standard resilience handlers for HTTP clients.

## Usage

```csharp
builder.AddServiceDefaults();
```

Or for specific scenarios:

```csharp
builder.AddLowLatencyServiceDefaults();
builder.AddBackendServiceDefaults();
```

## Configuration

- `OTEL_EXPORTER_OTLP_ENDPOINT`: If set, enables OTLP exporter for OpenTelemetry.
