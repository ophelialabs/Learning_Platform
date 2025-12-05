using LP_app.Components;
using LP_app.Data;
using LP_app.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure Database
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    // ?? "Data Source=learning_platform.db";

// builder.Services.AddDbContext<LearningPlatformContext>(options =>
    // options.UseSqlServer(connectionString));

// Add API Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS for API access
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Apply pending migrations, create database, and seed data
// using (var scope = app.Services.CreateScope())
// {
    //var dbContext = scope.ServiceProvider.GetRequiredService<LearningPlatformContext>();
    //dbContext.Database.Migrate();
    //await SeedDataService.SeedAsync(dbContext);
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
