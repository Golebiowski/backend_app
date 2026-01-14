using MediatR;
using backend_app.Models;

namespace backend_app.Features.Todos.Queries
{
    public record GetTodosQuery() : IRequest<List<TodoItem>>;

    public class GetTodosQueryHandler
    {

    }
}
