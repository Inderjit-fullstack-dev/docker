using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DockerisedTodoApp.Data;
using DockerisedTodoApp.Entity;

namespace DockerisedTodoApp.Services
{
    public class TodoService : ITodoService
    {
        private readonly ApplicationDbContext _db;

        public TodoService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<Todo>> GetAllAsync()
        {
            return await _db.Todos.AsNoTracking().OrderBy(t => t.Id).ToListAsync();
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            return await _db.Todos.FindAsync(id);
        }

        public async Task<Todo> CreateAsync(Todo todo)
        {
            _db.Todos.Add(todo);
            await _db.SaveChangesAsync();
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
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _db.Todos.FindAsync(id);
            if (existing == null) return false;

            _db.Todos.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
