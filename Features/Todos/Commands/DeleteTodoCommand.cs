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
        private readonly ILogger<DeleteTodoCommandHandler> _logger;

        public DeleteTodoCommandHandler(AppDbContext context, ILogger<DeleteTodoCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting Todo id: {Id}", request.id);

            // Szukamy encji w bazie
            var item = await _context.Todos.FindAsync(new object[] { request.id }, cancellationToken);

            if ( item == null)
            {
                _logger.LogWarning("Todo id: {Id} not found", request.id);
                return false;
            }

            _context.Todos.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Todo id: {Id} deleted successfully", request.id);
            return true;
        }
    }
}
