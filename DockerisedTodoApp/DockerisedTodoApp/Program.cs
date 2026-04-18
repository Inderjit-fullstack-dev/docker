using Microsoft.EntityFrameworkCore;
using System.Linq;
using DockerisedTodoApp.Data;
using DockerisedTodoApp.Entity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

// Configure EF Core DbContext with SQL Server
var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection");
// Allow overriding connection string via environment when running in containers (uses Docker compose env vars)
var connFromEnv = builder.Configuration["ConnectionStrings:DefaultConnection"];
var sqlConn = !string.IsNullOrWhiteSpace(connFromEnv) ? connFromEnv : defaultConn;
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(sqlConn, sqlOptions =>
        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));

// Register TodoService
builder.Services.AddScoped<DockerisedTodoApp.Services.ITodoService, DockerisedTodoApp.Services.TodoService>();

// Configure Redis distributed cache (assumes redis running on localhost:6379)
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetValue<string>("Redis:Configuration");
});


builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Apply EF Core migrations at startup
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var maxAttempts = 10;
    var attempt = 0;
    var delay = TimeSpan.FromSeconds(5);
    while (true)
    {
        try
        {
            attempt++;
            logger.LogInformation("Attempt {Attempt} to apply migrations.", attempt);
            db.Database.Migrate();

            // Seed dummy records if none exist
            if (!db.Todos.Any())
            {
                db.Todos.AddRange(
                    new Todo { Title = "Buy groceries", Description = "Milk, Eggs, Bread", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(3) },
                    new Todo { Title = "Walk the dog", Description = "Evening walk in the park", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(1) },
                    new Todo { Title = "Pay bills", Description = "Electricity and Internet", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(7) },
                    new Todo { Title = "Read a book", Description = "Finish reading current novel", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(14) },
                    new Todo { Title = "Clean kitchen", Description = "Wipe surfaces and mop floor", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(2) }
                );

                db.SaveChanges();
            }

            break; // success
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Migration attempt {Attempt} failed.", attempt);
            if (attempt >= maxAttempts)
            {
                logger.LogError(ex, "An error occurred while migrating or initializing the database after {Attempts} attempts.", attempt);
                throw;
            }
            await Task.Delay(delay);
            // increase delay up to a cap
            delay = TimeSpan.FromSeconds(Math.Min(delay.TotalSeconds * 2, 30));
        }
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
