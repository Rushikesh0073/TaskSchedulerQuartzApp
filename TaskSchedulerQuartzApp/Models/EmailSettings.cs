namespace TaskSchedulerQuartzApp.Models
{
    public class EmailSettings
    {

        public string FromName { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ToEmails { get; set; } = string.Empty;


    }
}
