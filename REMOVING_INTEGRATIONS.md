# Removing Unused Integrations

If you don't need certain integrations, follow these steps:

## Remove Redis

1. Delete: `BackendTemplate.Infrastructure/Integrations/Redis/`
2. Delete: `BackendTemplate.Application/Services/Redis/`
3. Remove from `ServicesCollectionExtensions.cs`:
   - `services.AddRedis(configuration);`
   - `services.AddScoped<IRedisService, RedisService>();`
   - Health check registration
4. Remove from `appsettings.json`: `"Redis"` connection string
5. Remove from `AppSettings.cs`: `RedisSettings` (if exists)

## Remove Cloudinary

1. Delete: `BackendTemplate.Infrastructure/Integrations/Cloudinary/`
2. Delete: `BackendTemplate.Application/Services/FilesUpload/`
3. Remove from `ServicesCollectionExtensions.cs`:
   - `services.Configure<CloudinarySettings>(...)`
   - `services.AddScoped<IFilesUploadService, CloudinaryService>();`
4. Remove from `appsettings.json`: `"Cloudinary"` section
5. Remove from `AppSettings.cs`: `CloudinarySettings` class

## Remove Email Service

1. Delete: `BackendTemplate.Infrastructure/Integrations/Email/`
2. Delete: `BackendTemplate.Application/Services/Email/`
3. Remove from `ServicesCollectionExtensions.cs`:
   - `services.Configure<SmtpSettings>(...)`
   - `services.AddScoped<IEmailService, EmailService>();`
4. Remove from `appsettings.json`: `"Smtp"` section
5. Remove from `AppSettings.cs`: `SmtpSettings` class

## Remove Stripe

1. Delete: `BackendTemplate.Infrastructure/Integrations/Stripe/`
2. Delete: `BackendTemplate.Application/Services/Payment/`
3. Remove from `ServicesCollectionExtensions.cs`:
   - `services.Configure<StripeSettings>(...)`
   - `services.AddScoped<IPaymentService, StripeService>();`
4. Remove from `appsettings.json`: `"Stripe"` section
5. Remove from `AppSettings.cs`: `StripeSettings` class

## Remove Hangfire

1. Delete: `BackendTemplate.Infrastructure/Integrations/Hangfire/`
2. Remove from `ServicesCollectionExtensions.cs`:
   - `services.Configure<HangfireSettings>(...)`
   - `services.AddHangfire(configuration);`
3. Remove from `Program.cs`:
   - Hangfire using statements
   - Hangfire dashboard configuration
4. Remove from `appsettings.json`: `"Hangfire"` section
5. Remove from `AppSettings.cs`: `HangfireSettings` class

## Remove Swagger

1. Remove from `WebApplicationBuilderExtensions.cs`:
   - `AddSwaggerGen` configuration
2. Remove from `Program.cs`:
   - `app.UseSwagger()`
   - `app.UseSwaggerUI(...)`

---

**After removing any integration:**
1. Clean solution: `dotnet clean`
2. Restore: `dotnet restore`
3. Build: `dotnet build`
4. Fix any remaining compilation errors
