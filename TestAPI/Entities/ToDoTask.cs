namespace TestAPI.Entities
{
    public class ToDoTask
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Completed { get; set; }
    }
}
