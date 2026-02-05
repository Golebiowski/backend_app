using backend_app.Common;
using backend_app.Enums;
using backend_app.Features.Todos;

namespace backend_app.Models
{
    public class TodoItem : BaseEntity
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Priorities Priority { get; private set; }
        public string? CreatedBy { get; set; } 

        public int CategoryId { get; private set; } // Foreign key
        public virtual Category Category { get; private set; } // Nawigacyjne właściwość

        // prywatny kontrultor dla EntityFramework - techniczny  
        private TodoItem() { }

        public TodoItem(string title, int category)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
            }

            Title = title;
            CategoryId = category;
            IsCompleted = false;
            CreatedAt = DateTime.UtcNow;
            Priority = Priorities.Medium;
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

            AddDomainEvent(new TodoCompletedEvent(this.Id, this.Title, DateTime.UtcNow));
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

        public void UpdateDetails(string newTitle, int newCategoryId)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title cannot be empty.");

            Title = newTitle;
            CategoryId = newCategoryId; 
        }
    }
}