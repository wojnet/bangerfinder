using Backend.bangerback.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
// Remove CookieAuthenticationDefaults if switching to pure JWT or custom header auth

var builder = WebApplication.CreateBuilder(args);

// 1. CHANGE: Use AddControllers (No Views)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Vital: Prevent JSON loops from EF Core Navigation Properties
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// 2. ADD: Swagger/OpenAPI (Essential for your React devs to know your endpoints)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database Context (Keep as is)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));


builder.Services.AddTransient<Backend.bangerback.Infrastructure.Services.EmailSender>();

// 3. ADD: CORS (React runs on a different port/domain in dev, e.g., localhost:3000)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:3000") // Adjust for your React URL
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

var app = builder.Build();

// Configure Pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 4. ADD: CORS Middleware (Must be before Auth)
app.UseCors("AllowReactApp");

// 5. REMOVE: Cookie Auth Middleware (We will use Token Headers)
// app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers(); // Maps API routes, not Controller routes

app.Run();