using backend_app.Features.Todos.Commands;
using FluentValidation;

namespace backend_app.Validators
{
    public class CreateTodoValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tytuł nie może być pusty.")
                .MinimumLength(3).WithMessage("Tytuł musi mieć co najmniej 3 znaki.")
                .MaximumLength(100).WithMessage("Tytuł nie może przekraczać 200 znaków.");
        }
    }
}
