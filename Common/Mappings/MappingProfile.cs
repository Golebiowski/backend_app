using AutoMapper;
using backend_app.Models;
using backend_app.Features.Todos.Queries;

namespace backend_app.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // "Stwórz mapę z TodoItem do TodoDto"
            CreateMap<TodoItem, TodoDto>()
                // Mapujemy Enum na jego tekstową reprezentację
                .ForMember(x => x.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt.ToString("dd-MM-yyyy HH:mm")))
                .ForCtorParam("CategoryName", opt => opt.MapFrom(src => src.Category.Name));
        }
    }
}
