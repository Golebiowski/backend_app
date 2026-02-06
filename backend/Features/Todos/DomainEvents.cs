using backend_app.Common; 

namespace backend_app.Features.Todos
{
    public record TodoCompletedEvent(int TodoId, string Title, DateTime CompletedAt) : IDomainEvent;

}
