using GmailOrganizer.Models;

namespace GmailOrganizer.Services
{
    public interface IMailServices
    {
        // public bool Login();
        Task<List<MessageInfo>> getMails();

        Task<List<MessageInfo>> deleteMails(string[] senders);
    }
}
