using MediatR;
using backend_app.Models;
using backend_app.Data;

namespace backend_app.Features.Todos.Commands
{
    public record UpdateTodoCommand(int Id, string Title, bool IsCompleted) : IRequest<bool>;

    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateTodoCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var item = await _context.Todos.FindAsync(new object[] {request.Id }, cancellationToken);

            if (item == null)
            {
                return false;
            }

            // Zamiast: item.Title = request.Title; (Anemiczne)
            // Robimy: (DDD - Delegujemy logikę do modelu)
            item.UpdateTitle(request.Title);
            //item.MarkAsCompleted();

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
