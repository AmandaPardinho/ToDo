using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDo.Data;
using ToDo.Models;
using ToDo.ViewModels;

//controller => recebe a requisição, manipula e devolve para a tela
namespace ToDo.Controllers
{
    [ApiController]
    [Route(template:"v1")]
    public class TodoController: ControllerBase
    {
        [HttpGet]
        [Route(template:"todos")]
        public async Task<IActionResult> GetAsync([FromServices] TodoDbContext context)
        {
            var todos = await context
                .Todos
                .AsNoTracking()
                .ToListAsync();
            return Ok(todos);
        }

        [HttpGet]
        [Route(template:"todos/{id}")]
        //a rota pode ser inserida separada ou com junto com o verbo http
        public async Task<IActionResult> GetByIdAsync([FromServices] TodoDbContext context, [FromRoute] int id)
        {
            var todo = await context
                .Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(tarefa => tarefa.Id == id);
            return todo == null ? NotFound() : Ok(todo);
        }

        [HttpPost(template: "todos")]
        public async Task<IActionResult> PostAsync([FromServices] TodoDbContext context, [FromBody] CreateTodoViewModel model)
        {
            if(!ModelState.IsValid) return BadRequest();

            var todo = new Todo
            {
                Date = DateTime.Now,
                Done = false,
                Title = model.Title
            };

            try
            {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created(uri:$"v1/todos/{todo.Id}", todo);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPut(template:"todos/{id}")]
        public async Task<IActionResult> PutAsync([FromServices] TodoDbContext context, [FromBody] CreateTodoViewModel model, [FromRoute] int id)
        {
            if(!ModelState.IsValid) return BadRequest();

            var todo = await context.Todos.FirstOrDefaultAsync(tarefa => tarefa.Id == id);

            if(todo == null) return NotFound();

            try
            {
                todo.Title = model.Title;
                
                context.Todos.Update(todo);
                await context.SaveChangesAsync();

                return Ok(todo);
            }
            catch (Exception e)
            {

                return BadRequest();
            }
        }

        [HttpDelete(template:"todos/{id}")]
        public async Task<IActionResult> DeleteAsync([FromServices] TodoDbContext context, [FromRoute] int id)
        {
            var todo = await context.Todos.FirstOrDefaultAsync(tarefa => tarefa.Id == id);

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();
                return Ok("Tarefa apagada com sucesso");

            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}
