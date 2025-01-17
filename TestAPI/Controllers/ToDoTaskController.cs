using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI.Contexts;
using TestAPI.Entities;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoTaskController(ILogger<ToDoTaskController> logger, ToDoTasksContext context) : ControllerBase
    {
        private readonly ILogger<ToDoTaskController> _logger = logger;
        private readonly ToDoTasksContext _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation($"");

            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Номер страницы и количество элементов должны быть больше 0");
            }

            var totalItems = await _context.ToDoTasks.CountAsync();
            var skip = (page - 1) * pageSize;

            if (skip >= totalItems)
            {
                return NotFound("По указанным параметрам элементов не найдено");
            }

            var tasks = await _context.ToDoTasks
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return Ok(tasks);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            var task = await _context.ToDoTasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ToDoTask newTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            newTask.Id = 0;

            _context.ToDoTasks.Add(newTask);

            await _context.SaveChangesAsync();

            return Ok(newTask);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] JsonPatchDocument<ToDoTask> patchDoc)
        {
            var task = await _context.ToDoTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            if (patchDoc.Operations.Any(x => x.path.ToLower() == "/id"))
            {
                return BadRequest("Невозможно изменить свойство Id");
            }

            patchDoc.ApplyTo(task, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.SaveChangesAsync();

            return Ok(task);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var task = await _context.ToDoTasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.ToDoTasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
