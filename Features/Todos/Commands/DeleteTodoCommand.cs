using MediatR;
using backend_app.Models;

namespace backend_app.Features.Todos.Commands
{
    //Command
    public record DeleteTodoCommand(int id) : IRequest<bool>;

    // Handler
    public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
    {
        public Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
        {
            var itemToRemove = FakeDataBase.Items.FirstOrDefault(t => t.Id == request.id);

            if ( itemToRemove == null)
            {
                return Task.FromResult(false);
            }

            FakeDataBase.Items.Remove(itemToRemove);
            return Task.FromResult(true);
        }
    }
}
