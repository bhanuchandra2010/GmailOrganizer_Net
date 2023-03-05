namespace GmailOrganizer.Models
{
    public class MessageInfo
    {
        public string? Id { get; set; }
        public string? Subject { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public bool IsSelected { get; set; }

    }
}
