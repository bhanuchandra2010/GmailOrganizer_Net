using GmailOrganizer.Models;

namespace GmailOrganizer.Services
{
    public interface IMailServices
    {
        // public bool Login();
        Task<List<MessageInfo>> getMails();

        Task DeleteMessages(string[] senders);
    }
}
