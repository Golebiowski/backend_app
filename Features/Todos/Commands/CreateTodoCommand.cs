using MediatR;
using backend_app.Models;

namespace backend_app.Features.Todos.Commands
{
    public record CreateTodoCommand(string Title) : IRequest<int>;

    public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, int>
    {
        public Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            var newTodo = new TodoItem
            {
                Id = FakeDataBase.Items.Count + 1,
                Title = request.Title
            };
            FakeDataBase.Items.Add(newTodo);
            return Task.FromResult(newTodo.Id);
        }
    }
}
