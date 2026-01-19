using backend_app.Data;
using backend_app.Models;
using MediatR;

namespace backend_app.Features.Todos.Commands
{
    //Command
    public record DeleteTodoCommand(int id) : IRequest<bool>;

    // Handler
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteTodoCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            // Szukamy encji w bazie
            var item = await _context.Todos.FindAsync(new object[] { request.id }, cancellationToken);

            if ( item == null)
            {
                return false;
            }

            _context.Todos.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
