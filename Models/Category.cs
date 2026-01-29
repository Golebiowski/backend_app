namespace backend_app.Models
{
    // Agregat kategorii zadań
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        // Kolekcja zadań przypisanych do tej kategorii
        private readonly List<TodoItem> _todos = new();
        public virtual IReadOnlyCollection<TodoItem> Todos => _todos.AsReadOnly();
    
        private Category() { } // Dla Entity Framework

        public Category(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be null or empty", nameof(name));
            }
            Name = name;
        }

    }
}
