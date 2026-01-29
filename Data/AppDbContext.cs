using Microsoft.EntityFrameworkCore;
using backend_app.Models;

namespace backend_app.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    { 
        // Ta linia mówi: "Chcę mieć tabelę o nazwie 'Todos' opartą na klasie TodoItem"
        public virtual DbSet<TodoItem> Todos { get; set; }

        public virtual DbSet<Category> Categories { get; set; }
    }
}
