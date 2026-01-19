using MediatR;
using Microsoft.AspNetCore.Mvc;
using backend_app.Features.Todos.Commands;
using backend_app.Features.Todos.Queries;
using backend_app.Enums;

namespace backend_app.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTodoCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpGet]
        public async Task<ActionResult<List<TodoDto>>> GetAll()
        {
            return Ok(await _mediator.Send(new GetTodosQuery()));
        }

        [HttpDelete("{id}")] // np. api/todo/1
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTodoCommand(id));
            if (!result)
            {
                return NotFound($"Zadanie o ID {id} nie istnieje.");
            }
            return NoContent(); //zwraca 204
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTodoCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("ID w adresie musi być zgodne z ID w danych.");
            }

            var result = await _mediator.Send(command);

            if (!result)
            {
                return NotFound($"Zadanie o ID {id} nie istnieje.");
            }

            return NoContent(); //zwraca 204
        }

        [HttpPatch("{id}/priority")]
        public async Task<IActionResult> UpdatePriority(int id, [FromBody] Priorities newPriority)
        { 
            var result = await _mediator.Send(new ChangePriorityCommand(id, newPriority));
            if (!result)
            {
                return NotFound($"Zadanie o ID {id} nie istnieje.");
            }
            return NoContent(); //zwraca 204
        }
    }
}
