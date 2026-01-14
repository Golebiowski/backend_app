using MediatR;
using backend_app.Models;

namespace backend_app.Features.Todos
{
    public record UpdateTodoCommand(int Id, string Title, bool IsCompleted) : IRequest<bool>;

    public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, bool>
    {
        public Task<bool> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
        {
            var item = FakeDataBase.Items.FirstOrDefault(t => t.Id == request.Id);

            if (item == null)
            {
                return Task.FromResult(false); /// W Handler robimy FromResult
            }

            item.Title = request.Title;
            item.IsCompleted = request.IsCompleted;

            return Task.FromResult(true);   
        }
    }
}
