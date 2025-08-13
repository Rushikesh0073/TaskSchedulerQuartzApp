namespace TaskSchedulerQuartzApp.Models
{
    public class EmailLog
    {
        public int Id { get; set; }
        public DateTime SentAtUtc { get; set; }
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
