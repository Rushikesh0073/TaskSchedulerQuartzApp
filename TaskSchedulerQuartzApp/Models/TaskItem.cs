using System.ComponentModel.DataAnnotations;

namespace TaskSchedulerQuartzApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [StringLength(2000)]
        public string? Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public bool IsCompleted { get; set; } = false;

    }
}
