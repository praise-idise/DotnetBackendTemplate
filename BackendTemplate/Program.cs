using Asp.Versioning.ApiExplorer;
using BackendTemplate.API.Extensions;
using BackendTemplate.Application.Extensions;
using BackendTemplate.Infrastructure.Extensions;
using BackendTemplate.Infrastructure.Persistence;
using BackendTemplate.Infrastructure.Seeder;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using SwaggerThemes;
using static BackendTemplate.Shared.Models.AppSettings;
using BackendTemplate.API.Middlewares;
using HangfireBasicAuthenticationFilter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddPresentation();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder);

builder.Services.AddControllers();

var app = builder.Build();

// Run seeders at startup
using (var scope = app.Services.CreateScope())
{

    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetService<IApplicationSeeder>();
    if (seeder != null)
    {
        await seeder.SeedAsync();
    }
}

// Configure the HTTP request pipeline.
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseSerilogRequestLogging();

app.UseCors("DefaultCorsPolicy");

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(Theme.UniversalDark, customStyles: null, setupAction: options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}
;

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

var hangfireSettings = app.Services.GetRequiredService<IOptions<HangfireSettings>>().Value;

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireCustomBasicAuthenticationFilter
    {
        User = hangfireSettings.DashboardUsername,
        Pass = hangfireSettings.DashboardPassword
    }],
    IgnoreAntiforgeryToken = true
});

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();
