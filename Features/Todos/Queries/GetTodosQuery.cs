using MediatR;
using backend_app.Models;
using backend_app.Data;
using Microsoft.EntityFrameworkCore;
using backend_app.Enums;

namespace backend_app.Features.Todos.Queries
{
    // 1. DTO - Prosty rekord tylko na potrzeby widoku
    public record TodoDto(int Id, string Title, bool IsCompleted, DateTime CreatedAt, Priorities Priority);

    // 2. Query - Teraz zwraca listę DTO, a nie Encji
    public record GetTodosQuery() : IRequest<List<TodoDto>>;

    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, List<TodoDto>>
    {
        private readonly AppDbContext _context;

        public GetTodosQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TodoDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            //return await _context.Todos.ToListAsync(cancellationToken);

            return await _context.Todos
            .Select(t => new TodoDto(t.Id, t.Title, t.IsCompleted, t.CreateAt, t.Priority))
            .ToListAsync(cancellationToken);
        }
    }
}
