# Clean Architecture Backend Template

A production-ready .NET 10 backend template with clean architecture and integrated services.

## Installation

### Install Template Locally
```bash
dotnet new install ./
```

### Install from NuGet (after publishing)
```bash
dotnet new install IdisePraise.BackendTemplate
```

## Usage

### Create New Project
```bash
dotnet new cleanapi -n MyAwesomeApi
```

The template includes all integrations by default. Simply remove the ones you don't need after project creation.

## What's Included

### Core Features (Always Included)
- Clean Architecture (Domain, Application, Infrastructure, API)
- JWT Authentication with refresh tokens
- Repository Pattern + Unit of Work
- Global Query Filters (soft delete)
- API Versioning
- CORS Configuration
- Rate Limiting
- Global Exception Handling
- EF Core + SQL Server
- Serilog Logging
- Health Checks

### Optional Features (Configurable)
- ✅ Redis (caching/sessions)
- ✅ Cloudinary (file uploads)
- ✅ SMTP Email
- ✅ Stripe (payments)
- ✅ Hangfire (background jobs)
- ✅ Swagger/OpenAPI

## After Creation

1. Update connection string in `appsettings.Development.json`
2. Configure integrations in `appsettings.json`
3. Run migrations: `dotnet ef database update`
4. Run the app: `dotnet run`

## Documentation

- `README.md` - Setup and usage guide
- `GLOBAL_QUERY_FILTERS.md` - Soft delete implementation
- `MULTITENANCY.md` - Multitenancy guide

## More Information

See `README.md` in the generated project for complete documentation.

## Uninstall

```bash
dotnet new uninstall IdisePraise.BackendTemplate
```
