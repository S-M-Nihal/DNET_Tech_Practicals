using Microsoft.EntityFrameworkCore;
using SmartCampusPortal.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Add MVC Controllers + Views AND Web API Controllers
builder.Services.AddControllersWithViews();

// 2. Register EF Core SQL Server DbContext
builder.Services.AddDbContext<CampusDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CampusDbConnection")));

var app = builder.Build();

// 3. Auto-Initialize Database and Seed Data on Startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CampusDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Enable attribute routing for REST API endpoints (/api/campus/...)
app.MapControllers();

// Set ServiceRequest/Index as the default opening landing page
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ServiceRequest}/{action=Index}/{id?}");

app.Run();