using MediatR;
using backend_app.Models;
using backend_app.Data;
using FluentAssertions;

namespace backend_app.Features.Todos.Commands
{
    public record CreateTodoCommand(string Title) : IRequest<int>;

    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, int>
    {
        private readonly AppDbContext _context;

        public CreateTodoCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            // Wywołujemy "mądry" konstruktor DDD. 
            // Jeśli Title będzie pusty, konstruktor wyrzuci błąd (wyjątek) tutaj.
            var toDoItem = new TodoItem(request.Title);

            _context.Todos.Add(toDoItem);
            await _context.SaveChangesAsync(cancellationToken);

            return toDoItem.Id;
        }
    }
}
