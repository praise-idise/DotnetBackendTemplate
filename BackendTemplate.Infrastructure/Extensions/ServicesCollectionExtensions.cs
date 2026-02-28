using BackendTemplate.Application.Services.Email;
using BackendTemplate.Application.Services.FilesUpload;
using BackendTemplate.Application.Services.Payment;
using BackendTemplate.Application.Services.Redis;
using BackendTemplate.Domain.Entities;
using BackendTemplate.Domain.Interfaces;
using BackendTemplate.Infrastructure.Integrations.Cloudinary;
using BackendTemplate.Infrastructure.Integrations.Email;
using BackendTemplate.Infrastructure.Integrations.Hangfire;
using BackendTemplate.Infrastructure.Integrations.Redis;
using BackendTemplate.Infrastructure.Integrations.Stripe;
using BackendTemplate.Infrastructure.Persistence;
using BackendTemplate.Infrastructure.Repositories;
using BackendTemplate.Infrastructure.Seeder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using static BackendTemplate.Shared.Models.AppSettings;

namespace BackendTemplate.Infrastructure.Extensions;

public static class ServicesCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection string is missing.")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
        })
          .AddRoles<IdentityRole>()
          .AddEntityFrameworkStores<ApplicationDbContext>()
          .AddDefaultTokenProviders();

        services.AddRedis(configuration);

        services.AddHealthChecks().AddRedis(configuration.GetConnectionString("Redis")!,
        name: "Redis",
        failureStatus: HealthStatus.Degraded
        );
        services.AddScoped<IRedisService, RedisService>();

        // Cloudinary
        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
        services.AddScoped<IFilesUploadService, CloudinaryService>();

        // SMTP Email
        services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
        services.AddScoped<IEmailService, EmailService>();

        // Stripe
        services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
        services.AddScoped<IPaymentService, StripeService>();

        // Hangfire
        services.Configure<HangfireSettings>(configuration.GetSection("Hangfire"));
        services.AddHangfire(configuration);

        // Seeders
        services.AddScoped<IApplicationSeeder, ApplicationSeeder>();

    }
}
