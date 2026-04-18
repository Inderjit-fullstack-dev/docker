using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using DockerisedTodoApp.Data;
using DockerisedTodoApp.Entity;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace DockerisedTodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _db;
        private readonly IDistributedCache _cache;
        private const string AllTodosCacheKey = "todos:all";

        public TodoService(ApplicationDbContext db)
        {
            _db = db;
            // IDistributedCache will be injected by DI; use service provider to get it if available
            // to keep constructor signature compatible, we'll inject via parameter overload below in DI registration
        }

        // Added constructor for DI to pass cache
        public TodoService(ApplicationDbContext db, IDistributedCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public async Task<List<Todo>> GetAllAsync()
        {
            if (_cache != null)
            {
                var cached = await _cache.GetAsync(AllTodosCacheKey);
                if (cached != null)
                {
                    var json = Encoding.UTF8.GetString(cached);
                    try
                    {
                        var todos = JsonSerializer.Deserialize<List<Todo>>(json);
                        if (todos != null) return todos;
                    }
                    catch
                    {
                        // ignore deserialization errors and fall through to DB
                    }
                }
            }

            var items = await _db.Todos.AsNoTracking().OrderBy(t => t.Id).ToListAsync();

            if (_cache != null)
            {
                var json = JsonSerializer.Serialize(items);
                await _cache.SetAsync(AllTodosCacheKey, Encoding.UTF8.GetBytes(json), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }

            return items;
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _db.Todos.FindAsync(id);
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
            if (_cache != null)
            {
                await _cache.RemoveAsync(AllTodosCacheKey);
            }
            return todo;
        }

        public async Task<bool> UpdateAsync(int id, Todo todo)
        {
            var existing = await _db.Todos.FindAsync(id);
            if (existing == null) return false;

            existing.Title = todo.Title;
            existing.Description = todo.Description;
            existing.IsCompleted = todo.IsCompleted;
            existing.DueDate = todo.DueDate;

            _db.Todos.Update(existing);
            await _db.SaveChangesAsync();
            if (_cache != null)
            {
                await _cache.RemoveAsync(AllTodosCacheKey);
            }
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Todos.FindAsync(id);
            if (existing == null) return false;

            _db.Todos.Remove(existing);
            await _db.SaveChangesAsync();
            if (_cache != null)
            {
                await _cache.RemoveAsync(AllTodosCacheKey);
            }
            return true;
        }
    }
}
