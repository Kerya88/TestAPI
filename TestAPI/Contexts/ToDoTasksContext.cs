using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TestAPI.Entities;

namespace TestAPI.Contexts
{
    public class ToDoTasksContext(DbContextOptions<ToDoTasksContext> options) : DbContext(options)
    {
        public DbSet<ToDoTask> ToDoTasks { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
    }
}
