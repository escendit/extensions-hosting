# Temporalio Hosting Extensions

[![NuGet Version](https://img.shields.io/nuget/v/Escendit.Extensions.Hosting.Temporalio.svg)](https://www.nuget.org/packages/Escendit.Extensions.Hosting.Temporalio)

Simplified hosting extensions for [Temporal.io](https://temporal.io/) .NET SDK.

## Features

- **HostApplicationBuilder Extensions** for Temporal Client and Workers.
- **Environment-based configuration** for gRPC endpoint, namespace, and task queue.
- **OpenTelemetry integration** for Temporal Client, Workflows, and Activities.

## Usage

### Add Temporal Client

```csharp
builder.AddTemporalClient();
```

### Add Temporal Worker (Hosted Service)

```csharp
builder.AddTemporalServerRuntime(worker =>
{
    worker.AddWorkflow<MyWorkflow>();
    worker.AddActivity<MyActivity>();
});
```

## Configuration

Required environment variables or configuration keys:

- `TEMPORAL_GRPC`: The gRPC endpoint (e.g., `localhost:7233`).
- `TEMPORAL_NAMESPACE`: The Temporal namespace (e.g., `default`).
- `TEMPORAL_QUEUE`: The task queue name (required for workers).
- `TEMPORAL_BUILD_ID`: Optional build ID for worker versioning.
