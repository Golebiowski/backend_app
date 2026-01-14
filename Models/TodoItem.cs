namespace backend_app.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    // baza w pamięci do zamienienie potem
    public static class FakeDataBase
    {
        public static List<TodoItem> Items = new();
    }

}
