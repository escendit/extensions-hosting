# Orleans Hosting Extensions

[![NuGet Version](https://img.shields.io/nuget/v/Escendit.Extensions.Hosting.Orleans.svg)](https://www.nuget.org/packages/Escendit.Extensions.Hosting.Orleans)

Simplified hosting extensions for [Microsoft Orleans](https://learn.microsoft.com/en-us/dotnet/orleans/).

## Features

- **Silo & Client configuration** with opinionated defaults.
- **Clustering:** ADO.NET, Redis, and Kubernetes.
- **Persistence:** ADO.NET.
- **Reminders:** ADO.NET, Redis.
- **Streaming:** NATS.
- **Event Sourcing** defaults.
- **Telemetry:** Automatic OpenTelemetry integration for Orleans.

## Usage

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder.AddOrleansServerRuntime(); // Configures Silo with defaults
// OR
builder.AddOrleansClientRuntime(); // Configures Client with defaults

var app = builder.Build();
app.Run();
```

## Configuration

- `Hosting`: Set to `Kubernetes` for Kubernetes hosting.
- `ClusterOptions`: Binds to `ClusterOptions` for Orleans.
