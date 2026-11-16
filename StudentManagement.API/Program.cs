using Microsoft.EntityFrameworkCore;
using StudentManagement.API.Data;
using StudentManagement.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. DEPENDENCY INJECTION (SERVICES)
// ==========================================

// Add Controller support
builder.Services.AddControllers();

// Add OpenAPI / Swagger Documentation Generator
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Entity Framework Core In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("StudentManagementDb"));

var app = builder.Build();

// Seed the In-Memory Database on initial startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

// ==========================================
// 2. HTTP REQUEST PIPELINE (MIDDLEWARE ORDER)
// ==========================================

// Custom Middlewares
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

// Configure Swagger for Development & Testing
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Standard route: /swagger
}

// Automatically redirect root URL (/) to Swagger UI (/swagger)
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();