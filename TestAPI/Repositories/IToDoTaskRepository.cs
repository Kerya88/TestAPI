using TestAPI.Entities;

namespace TestAPI.Repositories
{
    public interface IToDoTaskRepository
    {
        IEnumerable<ToDoTask> GetAll();
        ToDoTask GetById(int id);
        void Create(ToDoTask task);
        void Update(ToDoTask task);
        void Delete(int id);
    }
}
