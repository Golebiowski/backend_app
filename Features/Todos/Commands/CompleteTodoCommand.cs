using backend_app.Data;
using MediatR;
using MediatR.Wrappers;

namespace backend_app.Features.Todos.Commands
{
    public record CompleteTodoCommand(int id) : IRequest<bool>;

    public class CompleteTodoCommandHandler : IRequestHandler<CompleteTodoCommand, bool>
    {
        private readonly AppDbContext _context; 
        private readonly ILogger _logger;

        public CompleteTodoCommandHandler(AppDbContext context, ILogger<CompleteTodoCommandHandler> logger)
        {
            _context = context; 
            _logger = logger;
        } 

        public async Task<bool> Handle(CompleteTodoCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.Todos.FindAsync(new object[] { request.id }, cancellationToken);
            if (todo == null)
            {
                _logger.LogWarning("Todo with ID {TodoId} not found.", request.id);
                return false;
            }

            todo.MarkAsCompleted();

            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Todo with ID {TodoId} marked as completed.", request.id);

            return true;
        }
    }
}
