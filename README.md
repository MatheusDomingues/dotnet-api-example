# .NET API Example

A REST API starter in ASP.NET Core 8 organized in layers, with Entity Framework Core and PostgreSQL.

## What's inside

- **Layered solution:** WebApi, Services, Domain, Data and CrossCutting
- **Versioned controllers** (`/api/v1/stock`, `/api/v1/comment`) with CRUD endpoints
- **Generic repository** and paginated queries
- **EF Core** mappings and migrations for PostgreSQL
- **Health checks** for the database context
- **Swagger** documentation
- **Docker** image and a Bitbucket Pipelines config

## Stack

C# · .NET 8 · ASP.NET Core · Entity Framework Core · PostgreSQL · Docker

## Getting started

Set `ConnectionStrings:CONNECTION_STRING` in `appsettings.json`, then:

```bash
dotnet ef database update
dotnet run
```
