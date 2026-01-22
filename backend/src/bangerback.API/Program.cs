using Backend.bangerback.Infrastructure.Data;
using bangerback.Infrastructure.Services;
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
builder.Services.AddHttpClient();
builder.Services.AddScoped<LastFmService>();

// Database Context (Keep as is)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));


builder.Services.AddTransient<Backend.bangerback.Infrastructure.Services.EmailSender>();

// 3. ADD: CORS (React runs on a different port/domain in dev, e.g., localhost:3000)
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowReactApp",
//    policy => policy.WithOrigins("http://localhost:3000", "http://localhost") // Added port 80
//                    .AllowAnyMethod()
//                    .AllowAnyHeader());
//    options.AddPolicy("DevPolicy", policy =>
//    {
//        policy.AllowAnyOrigin() // P
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy =>
        policy.SetIsOriginAllowed(origin => true) // More resilient for dev
              .AllowAnyMethod()
              .AllowAnyHeader());
});


// order is critical: app.UseCors MUST come before app.MapControllers()


var app = builder.Build();

// those lines are yoused by docker to do the migration
// --- START OF RESILIENT MIGRATION LOGIC ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>();

    int retries = 5;
    while (retries > 0)
    {
        try
        {
            logger.LogInformation("Attempting to apply migrations... ({0} attempts left)", retries);
            context.Database.Migrate();
            logger.LogInformation("Database migration successful!");
            break; // Exit loop on success
        }
        catch (Exception ex)
        {
            retries--;
            logger.LogWarning("Database not ready yet. Retrying in 10 seconds... Error: {0}", ex.Message);
            if (retries == 0)
            {
                logger.LogError(ex, "Could not migrate database after multiple attempts.");
            }
            System.Threading.Thread.Sleep(10000); // Wait 10 seconds
        }
    }
}
// --- END OF RESILIENT MIGRATION LOGIC ---

//

// app.UseCors("DevPolicy"); - this line gives a ton of freedom, comparing to AllowReactApp

// Configure Pipeline
if (app.Environment.IsDevelopment())
{
    // Enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// 4. ADD: CORS Middleware (Must be before Auth)
 app.UseCors("AllowReactApp");

// 5. REMOVE: Cookie Auth Middleware (We will use Token Headers)
// app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers(); // Maps API routes, not Controller routes

app.Run();