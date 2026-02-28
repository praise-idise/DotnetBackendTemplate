# Dotnet Backend Template

A comprehensive .NET 10 backend template with clean architecture, ready-to-use integrations, and production-ready patterns.

## Architecture

### Clean Architecture Layers

```
BackendTemplate/
├── BackendTemplate.API/              # Presentation Layer
│   ├── Controllers/                  # API endpoints
│   ├── Middlewares/                  # Custom middleware
│   └── Extensions/                   # Service configuration
├── BackendTemplate.Application/      # Application Layer
│   ├── Services/                     # Business logic
│   ├── Validators/                   # FluentValidation rules
│   └── Extensions/                   # DI registration
├── BackendTemplate.Infrastructure/   # Infrastructure Layer
│   ├── Persistence/                  # EF Core DbContext
│   ├── Repositories/                 # Data access
│   ├── Integrations/                 # External services
│   └── Seeder/                       # Data seeding
├── BackendTemplate.Domain/           # Domain Layer
│   ├── Entities/                     # Business entities
│   └── Interfaces/                   # Contracts
└── BackendTemplate.Shared/           # Shared Layer
    ├── Models/                       # DTOs, settings
    └── Services/                     # Cross-cutting concerns
```

### Key Patterns

- **Repository Pattern + Unit of Work**: Clean data access with transaction support
- **CQRS-Ready**: Service layer ready for command/query separation
- **Global Query Filters**: Automatic soft delete and active status filtering
- **Dependency Injection**: All services registered and injectable
- **Service Response Pattern**: Consistent API responses with status codes

---

## Features

### Core Infrastructure
- ✅ **Clean Architecture** - Separation of concerns
- ✅ **Repository Pattern** with Unit of Work
- ✅ **JWT Authentication** with refresh tokens & token versioning
- ✅ **Global Query Filters** - Soft delete (`IsDeleted`, `IsActive`)
- ✅ **API Versioning** (v1, v2, etc.)
- ✅ **CORS** configured
- ✅ **Rate Limiting** (100 req/min per IP)
- ✅ **Global Exception Handling** - Consistent error responses
- ✅ **Health Checks** endpoint

### Database & ORM
- ✅ **Entity Framework Core 10** with SQL Server
- ✅ **Auto-migrations** on startup
- ✅ **Soft Delete** support (`IsDeleted`)
- ✅ **Audit Fields** (`CreatedAt`, `UpdatedAt`, `IsActive`)
- ✅ **Seeder Infrastructure** for initial data

### API & Documentation
- ✅ **Swagger/OpenAPI** with JWT authentication
- ✅ **API Response Wrapper** (`ServiceResponse<T>`)
- ✅ **Pagination** support (`PaginationResult<T>`)
- ✅ **FluentValidation** infrastructure

### Logging & Monitoring
- ✅ **Serilog** (console + file)
- ✅ **Request/Response Logging** middleware
- ✅ **Structured Logging** (JSON format)

### Integrations (Optional)
- ✅ **Redis** - Caching and session management
- ✅ **Cloudinary** - File and image uploads
- ✅ **SMTP Email** - Email service (Gmail, SendGrid, etc.)
- ✅ **Stripe** - Payment processing
- ✅ **Hangfire** - Background job processing with dashboard

---

## Quick Start

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- SQL Server (LocalDB/Express/Developer)
- Redis (optional, for caching)

### Installation

```bash
# Clone repository
git clone https://github.com/praise-idise/DotnetBackendTemplate.git
cd DotnetBackendTemplate

# Restore packages
dotnet restore

# Update connection string in appsettings.Development.json
# See Configuration section below

# Run (migrations run automatically on startup)
cd BackendTemplate
dotnet run
```

### Endpoints
- **API**: `https://localhost:7000`
- **Swagger**: `https://localhost:7000/swagger`
- **Hangfire**: `https://localhost:7000/hangfire`
- **Health**: `https://localhost:7000/health`

### Default Admin Account
- **Email**: `admin@example.com`
- **Password**: `AdminUser123$`

---

## Configuration

### Required: Database Connection

Update `appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.\\SQLEXPRESS;Database=YourAppDB;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true",
  "Redis": "localhost:6379,abortConnect=false,connectRetry=3,connectTimeout=5000,syncTimeout=5000"
}
```

### Optional: Integrations

#### Cloudinary (File/Image Uploads)
```json
"Cloudinary": {
  "CloudName": "your-cloud-name",
  "ApiKey": "your-api-key",
  "ApiSecret": "your-api-secret",
  "FolderPrefix": "YourAppName"
}
```
Sign up: [cloudinary.com](https://cloudinary.com/)

#### SMTP Email Service
```json
"Smtp": {
  "Host": "smtp.gmail.com",
  "Port": 465,
  "Username": "your-email@gmail.com",
  "Password": "your-app-password",
  "FromAddress": "your-email@gmail.com",
  "FromName": "YourApp"
}
```
**Gmail Setup**: Enable 2FA → [Generate App Password](https://myaccount.google.com/apppasswords)

#### Stripe Payments
```json
"Stripe": {
  "SecretKey": "sk_test_your_secret_key",
  "PublishableKey": "pk_test_your_publishable_key",
  "WebhookSecret": "whsec_your_webhook_secret"
}
```
Get test keys: [stripe.com/dashboard](https://dashboard.stripe.com/)

#### Hangfire Background Jobs
```json
"Hangfire": {
  "DashboardUsername": "admin",
  "DashboardPassword": "secure_password"
}
```

#### CORS (Frontend Integration)
```json
"Cors": {
  "AllowedOrigins": ["http://localhost:3000", "https://your-frontend.com"],
  "AllowCredentials": true
}
```

---

## Usage Guide

### Creating an Entity

All entities should inherit from `BaseEntity` and implement a named ID with `[Key]` attribute:

```csharp
public class Product : BaseEntity
{
    [Key]
    public Guid ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
```

**BaseEntity provides:**
- `CreatedAt` - Timestamp
- `UpdatedAt` - Timestamp
- `IsDeleted` - Soft delete flag
- `IsActive` - Active status flag

### Using Repository Pattern

```csharp
public class ProductService
{
    private readonly IUnitOfWork _unitOfWork;
    
    public ProductService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    // Get by ID
    public async Task<Product?> GetProductAsync(Guid id)
    {
        return await _unitOfWork.Repository<Product>().GetByIdAsync(id);
    }
    
    // Get with pagination
    public async Task<PaginationResult<Product>> GetProductsAsync(RequestParameters parameters)
    {
        return await _unitOfWork.Repository<Product>().GetPagedItemsAsync(
            parameters,
            query => query.OrderBy(p => p.Name)
        );
    }
    
    // Create
    public async Task<ServiceResponse> CreateProductAsync(Product product)
    {
        await _unitOfWork.Repository<Product>().AddAsync(product);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Created("Product created");
    }
    
    // Update
    public async Task<ServiceResponse> UpdateProductAsync(Product product)
    {
        _unitOfWork.Repository<Product>().Update(product);
        await _unitOfWork.SaveChangesAsync();
        return ServiceResponse.Success("Product updated");
    }
    
    // Soft delete
    public async Task<ServiceResponse> DeleteProductAsync(Guid id)
    {
        var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
        if (product == null)
            return ServiceResponse.NotFound("Product not found");
            
        product.IsDeleted = true;
        product.DeletedAt = DateTime.UtcNow;
        _unitOfWork.Repository<Product>().Update(product);
        await _unitOfWork.SaveChangesAsync();
        
        return ServiceResponse.Success("Product deleted");
    }
}
```

### Global Query Filters

All queries automatically exclude deleted and inactive records:

```csharp
// Automatically filters: IsDeleted = false AND IsActive = true
var products = await _unitOfWork.Repository<Product>().GetAllAsync();

// To include deleted/inactive records:
var allProducts = await _unitOfWork.Repository<Product>().GetAllIncludingDeletedAsync();
var product = await _unitOfWork.Repository<Product>().GetByIdIncludingDeletedAsync(id);
```

See `GLOBAL_QUERY_FILTERS.md` for details.

### Creating a Controller

```csharp
[ApiVersion("1.0")]
public class ProductsController : BaseController
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var result = await _productService.GetProductAsync(id);
        return ComputeResponse(result);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] RequestParameters parameters)
    {
        var result = await _productService.GetProductsAsync(parameters);
        return ComputeResponse(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO dto)
    {
        var result = await _productService.CreateAsync(dto);
        return ComputeResponse(result);
    }
}
```

**BaseController provides:**
- `ComputeResponse()` - Converts `ServiceResponse` to `IActionResult`
- `ValidateModelState()` - Model validation helper

### Background Jobs (Hangfire)

```csharp
// Fire and forget
BackgroundJob.Enqueue(() => SendWelcomeEmail(userId));

// Delayed execution
BackgroundJob.Schedule(() => SendReminder(userId), TimeSpan.FromHours(1));

// Recurring job
RecurringJob.AddOrUpdate(
    "daily-cleanup",
    () => CleanupOldData(),
    Cron.Daily
);

// With parameters
BackgroundJob.Enqueue<IEmailService>(x => x.SendEmailAsync(
    "user@example.com",
    "Welcome!",
    "Thanks for signing up!",
    CancellationToken.None
));
```

### Sending Emails

```csharp
// Plain text
await _emailService.SendEmailAsync(
    "user@example.com", 
    "Welcome!", 
    "Thanks for signing up!",
    cancellationToken
);

// HTML email
await _emailService.SendHtmlEmailAsync(
    "user@example.com",
    "Welcome!",
    "<h1>Welcome to our app!</h1><p>Thanks for joining!</p>",
    cancellationToken
);
```

### File Uploads (Cloudinary)

```csharp
// Upload multiple images
var urls = await _filesUploadService.UploadImagesAsync(
    files.Select(f => (f.OpenReadStream(), f.FileName)),
    cancellationToken
);

// Upload single image
var url = await _filesUploadService.UploadImageAsync(
    file.OpenReadStream(),
    file.FileName,
    cancellationToken
);
```

### Payment Processing (Stripe)

```csharp
// Create payment intent
var paymentIntent = await _paymentService.CreatePaymentIntentAsync(
    new CreatePaymentIntentDTO
    {
        Amount = 5000, // $50.00 (in cents)
        Currency = "usd",
        Description = "Order #12345"
    },
    cancellationToken
);

// Check payment status
var status = await _paymentService.GetPaymentIntentAsync(paymentIntent.Id, ct);

// Refund
await _paymentService.RefundPaymentAsync(
    new RefundDTO { PaymentIntentId = paymentIntent.Id },
    cancellationToken
);
```

---

## Authentication

### Register
```http
POST /api/v1/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "firstName": "John",
  "lastName": "Doe"
}
```

### Login
```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

**Response:**
```json
{
  "success": true,
  "statusCode": 200,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "abc123...",
    "expiresAt": "2024-01-01T12:00:00Z"
  }
}
```

### Using JWT in Swagger
1. Click **Authorize** button
2. Enter: `Bearer <your-token>`
3. Click **Authorize**

---

## Database Migrations

```bash
# Add migration
dotnet ef migrations add MigrationName --project BackendTemplate.Infrastructure --startup-project BackendTemplate

# Update database
dotnet ef database update --project BackendTemplate.Infrastructure --startup-project BackendTemplate

# Remove last migration
dotnet ef migrations remove --project BackendTemplate.Infrastructure --startup-project BackendTemplate
```

---

## Production Deployment

### Checklist

- [ ] Change JWT `SecretKey` in production
- [ ] Update admin credentials
- [ ] Configure production database connection
- [ ] Set production CORS origins
- [ ] Configure production SMTP settings
- [ ] Use Stripe **live keys** (not test keys)
- [ ] Secure Hangfire dashboard credentials
- [ ] Set environment variables for secrets (don't commit)
- [ ] Review rate limiting settings
- [ ] Configure logging aggregation (Application Insights, Seq)
- [ ] Enable HTTPS redirect
- [ ] Configure production Redis instance
- [ ] Set up database backups
- [ ] Configure health check monitoring

### Environment Variables

For sensitive data, use environment variables instead of `appsettings.json`:

```bash
export ConnectionStrings__DefaultConnection="Server=prod;Database=MyApp;..."
export JwtSettings__SecretKey="your-production-secret"
export Stripe__SecretKey="sk_live_..."
```

---

## Additional Documentation

- **GLOBAL_QUERY_FILTERS.md** - Soft delete implementation guide
- **MULTITENANCY.md** - How to add multitenancy support

---

## Tech Stack

- **.NET 10** / **ASP.NET Core 10** / **EF Core 10**
- **SQL Server** - Primary database
- **Redis** - Caching and sessions
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation
- **Mapster** - Object mapping
- **FluentValidation** - Input validation
- **Hangfire** - Background jobs

---

## Support

For issues or questions:
1. Check the documentation files in this repository
2. Review integration documentation links above
3. Contact: idisepraise@gmail.com

---

Happy coding! 🚀
