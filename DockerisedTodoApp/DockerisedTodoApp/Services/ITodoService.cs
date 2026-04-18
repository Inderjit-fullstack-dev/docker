using System.Collections.Generic;
using System.Threading.Tasks;
using DockerisedTodoApp.Entity;

namespace DockerisedTodoApp.Services
{
    public interface ITodoService
    {
        Task<List<Todo>> GetAllAsync();
        Task<Todo?> GetByIdAsync(int id);
        Task<Todo> CreateAsync(Todo todo);
        Task<bool> UpdateAsync(int id, Todo todo);
        Task<bool> DeleteAsync(int id);
    }
}
