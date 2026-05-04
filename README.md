# Hosting Extensions for .NET Applications

[![Build Status](https://github.com/escendit/extensions-hosting/actions/workflows/build.yml/badge.svg)](https://github.com/escendit/extensions-hosting/actions/workflows/build.yml)
[![NuGet Version](https://img.shields.io/nuget/v/Escendit.Extensions.Hosting.Orleans.svg)](https://www.nuget.org/packages/Escendit.Extensions.Hosting.Orleans)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)

This repository provides streamlined hosting extensions for .NET applications, offering simplified configuration and dependency injection setup for building robust, distributed applications. It provides opinionated defaults and convenient extension methods for integrating popular services like Temporal.io, Orleans, and OpenTelemetry.

## Key Features

- **Simplified Temporal.io integration** with hosting-friendly client and worker configuration.
- **Orleans clustering, persistence, and streaming setup** (including NATS integration).
- **Service Defaults** including OpenTelemetry (Metrics, Tracing, Logging), Health Checks, and Service Discovery.
- **UserSecrets management** for local development.
- **Production-ready defaults** for common use cases.

## Tech Stack

- **Language:** C# 14
- **Framework:** .NET 10 (ASP.NET Core)
- **Package Manager:** NuGet with Central Package Management (CPM)
- **Key Libraries:** Orleans, Temporal.io, OpenTelemetry, Polly (Resilience)

## Project Structure

- `src/Orleans`: Extensions for Microsoft Orleans.
- `src/Temporalio`: Extensions for Temporal.io SDK.
- `src/ServiceDefaults`: Shared defaults for OpenTelemetry, Health Checks, and Service Discovery.
- `src/UserSecrets`: Extensions for working with .NET User Secrets.

## Getting Started

### Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Installation

Install the desired package via NuGet:

```bash
dotnet add package Escendit.Extensions.Hosting.Orleans
# OR
dotnet add package Escendit.Extensions.Hosting.Temporalio
```

### Usage Example (Service Defaults)

In your `Program.cs`:

```csharp
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults(); // Configures OTEL, Health Checks, etc.

var app = builder.Build();
app.Run();
```

## Scripts & Commands

- **Build:** `dotnet build`
- **Test:** `dotnet test` (TODO: Add tests to the project)
- **Pack:** `dotnet pack`

## Environment Variables

- `OTEL_EXPORTER_OTLP_ENDPOINT`: Endpoint for OpenTelemetry OTLP exporter.
- `ASPNETCORE_ENVIRONMENT`: Used to determine environment-specific configurations.

## License

Licensed to the Escendit GmbH under one or more agreements.
The Escendit GmbH licenses this file to you under the Apache License 2.0.
See the [LICENSE](LICENSE) file for more information.
