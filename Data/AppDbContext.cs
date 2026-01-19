using Microsoft.EntityFrameworkCore;
using backend_app.Models;

namespace backend_app.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    { 
        // Ta linia mówi: "Chcę mieć tabelę o nazwie 'Todos' opartą na klasie TodoItem"
        public DbSet<TodoItem> Todos { get; set; }
    }
}
