using MediatR;
using backend_app.Models;
using backend_app.Data;
using Microsoft.EntityFrameworkCore;
using backend_app.Enums;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace backend_app.Features.Todos.Queries
{
    // 1. DTO - Prosty rekord tylko na potrzeby widoku
    public record TodoDto(int Id, string Title, bool IsCompleted, DateTime CreatedAt, Priorities Priority, string CategoryName);

    // 2. Query - Teraz zwraca listę DTO, a nie Encji
    public record GetTodosQuery() : IRequest<List<TodoDto>>;

    public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, List<TodoDto>>
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public GetTodosQueryHandler(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TodoDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
        {
            //return await _context.Todos.ToListAsync(cancellationToken);

            return await _context.Todos
            .Include(t => t.Category) // Ładujemy dane powiązanej kategorii (JOIN w SQL)
            .ProjectTo<TodoDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        }
    }
}
