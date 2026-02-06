using backend_app.Common.Interfaces;
using backend_app.Common.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace backend_app.Features.Todos
{
    // To jest "odbiornik" zdarzenia - tutaj dzieje się akcja poboczna
    public class TodoCompletedEventHandler : INotificationHandler<TodoCompletedEvent>
    {
        private readonly ILogger<TodoCompletedEventHandler> _logger;
        private readonly ICurrentUserService _currentUserService;

        public TodoCompletedEventHandler(ILogger<TodoCompletedEventHandler> logger, ICurrentUserService currentUserService)
        {
             _logger = logger;
            _currentUserService = currentUserService;
        }

        public Task Handle(TodoCompletedEvent notification, CancellationToken cancellationToken)
        {
            // Tutaj moglibyśmy wysłać e-mail, powiadomienie Push lub zaktualizować statystyki
            _logger.LogInformation("Todo with Title {Title} was completed at {CompletedAt} by user {UserName}", notification.Title, notification.CompletedAt, _currentUserService.UserName);
        
            return Task.CompletedTask;
        }
    }
}
