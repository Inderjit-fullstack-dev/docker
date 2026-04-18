using Microsoft.EntityFrameworkCore;
using System.Linq;
using DockerisedTodoApp.Data;
using DockerisedTodoApp.Entity;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

// Configure EF Core DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
        throw;
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
