using backend_app.Enums;

namespace backend_app.Models
{
    public class TodoItem
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public bool IsCompleted { get; private set; }
        public DateTime CreateAt { get; private set; }
        public Priorities Priority { get; private set; }

        // prywatny kontrultor dla EntityFramework - techniczny  
        private TodoItem() { }

        public TodoItem(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
            }

            Title = title;
            IsCompleted = false;
            CreateAt = DateTime.UtcNow;
        }

        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException("Title cannot be null or empty", nameof(newTitle));
            }
            Title = newTitle;
        }

        public void MarkAsCompleted()
        {
            if (IsCompleted) 
                return;

            IsCompleted = true;
        }

        public void ChangePriority(Priorities newPriority)
        {
            if (!IsCompleted)
            { 
                Priority = newPriority; 
            }
            else
            {
                throw new InvalidOperationException("Cannot change priority of a completed task.");
            }
        }
    }
}