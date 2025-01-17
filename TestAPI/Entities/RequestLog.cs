namespace TestAPI.Entities
{
    public class RequestLog
    {
        public long Id { get; set; }
        public string IPAddress { get; set; }
        public string Path { get; set; }
        public DateTime Date { get; set; }
    }
}
