using backend_app.Data;
using backend_app.Enums;
using MediatR;

namespace backend_app.Features.Todos.Commands
{
    public record ChangePriorityCommand(int Id, Priorities NewPriority) : IRequest<bool>;

    public class ChangePriorityCommandHandler : IRequestHandler<ChangePriorityCommand, bool>
    {
        private readonly AppDbContext _context;

        public ChangePriorityCommandHandler(AppDbContext context) =>  _context = context; 

        public async Task<bool> Handle(ChangePriorityCommand request, CancellationToken cancellationToken)
        {
            var todo = await _context.Todos.FindAsync(new object[] { request.Id }, cancellationToken);
        
            if (todo == null)
            {
                return false;
            }

            // TUTAJ wywołujemy Twoją metodę DDD!
            // Jeśli zadanie jest ukończone, poleci InvalidOperationException
            todo.ChangePriority(request.NewPriority);

            await _context.SaveChangesAsync(cancellationToken); 

            return true; 
        }
    }
}
