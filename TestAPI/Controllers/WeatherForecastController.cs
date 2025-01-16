using Microsoft.AspNetCore.Mvc;

namespace TestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        private readonly ILogger<ToDoController> _logger;

        public ToDoController(ILogger<ToDoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            // Общее количество элементов
            var totalItems = await _context.ToDoTasks.CountAsync();

            // Вычисляем количество пропущенных элементов
            var skip = (page - 1) * pageSize;

            if (skip >= totalItems)
            {
                return NotFound("No items found for the specified page.");
            }

            // Получаем элементы с учётом пагинации
            var tasks = await _context.ToDoTasks
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            // Формируем ответ с метаинформацией
            var response = new
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize),
                Data = tasks
            };

            return Ok(response);
        }


        [HttpGet("{id}")]
        public ActionResult<ToDoTask> Get(int id)
        {
            var task = _repository.GetById(id);
            if (task == null)
            {
                return NotFound();
            }
            return Ok(task);
        }

        [HttpPost]
        public ActionResult Create(ToDoTask task)
        {
            if (task == null)
            {
                return BadRequest();
            }

            _repository.Create(task);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchToDoTask(int id, JsonPatchDocument<ToDoTask> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest("Invalid patch document.");
            }

            // Найти задачу в базе данных (например, через контекст Entity Framework)
            var existingTask = await _context.ToDoTasks.FindAsync(id);

            if (existingTask == null)
            {
                return NotFound("Task not found.");
            }

            // Применить изменения из patchDoc к существующему объекту
            patchDoc.ApplyTo(existingTask, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Сохранить изменения
            _context.Entry(existingTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingTask);
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var existingTask = _repository.GetById(id);
            if (existingTask == null)
            {
                return NotFound();
            }

            _repository.Delete(id);
            return NoContent();
        }
    }
}
