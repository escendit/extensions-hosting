# UserSecrets Hosting Extensions

[![NuGet Version](https://img.shields.io/nuget/v/Escendit.Extensions.Hosting.UserSecrets.svg)](https://www.nuget.org/packages/Escendit.Extensions.Hosting.UserSecrets)

Simplified hosting extensions for working with .NET [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) during development.

## Usage

```csharp
builder.AddUserSecrets();
```

This adds User Secrets to the configuration if the environment is `Development`.
