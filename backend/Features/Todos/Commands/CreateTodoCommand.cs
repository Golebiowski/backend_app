using MediatR;
using backend_app.Models;
using backend_app.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace backend_app.Features.Todos.Commands
{
    public record CreateTodoCommand(string Title, int CategoryId, string? UserId) : IRequest<int>;

    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, int>
    {
        private readonly AppDbContext _context;

        public CreateTodoCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

            if(!categoryExists)
            {
                throw new InvalidOperationException($"Category with Id {request.CategoryId} does not exist.");
            }   

            // Wywołujemy "mądry" konstruktor DDD. 
            // Jeśli Title będzie pusty, konstruktor wyrzuci błąd (wyjątek) tutaj.
            var toDoItem = new TodoItem(request.Title, request.CategoryId, request.UserId);

            _context.Todos.Add(toDoItem);
            await _context.SaveChangesAsync(cancellationToken);

            return toDoItem.Id;
        }
    }
}
