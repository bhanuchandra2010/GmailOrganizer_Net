using GmailOrganizer.Controllers;
using GmailOrganizer.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Globalization;
using System.Runtime.Serialization;

namespace GmailOrganizer.Services
{
    public class MailServices : IMailServices
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GmailService? _gmailService;

        private static List<MessageInfo> messageInfos = new();
        public MailServices(ILogger<HomeController> logger)
        {
            _logger = logger;
            _gmailService = Login();
        }
        private GmailService Login()
        {
            string[] Scopes = { GmailService.Scope.MailGoogleCom };
            UserCredential credential;
            using (FileStream stream = new(
                "./client_secret.json",
                FileMode.Open,
                FileAccess.Read
            ))
            {
                string credPath = "token_Send";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                             GoogleClientSecrets.FromStream(stream).Secrets,
                              Scopes,
                              "user",
                              CancellationToken.None,
                              new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            // Create Gmail API service.
            return new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "mycodebit",
            });
        }
        public async Task<List<MessageInfo>> getMails()
        {
            ListMessagesResponse listMessagesResponse = await _gmailService.Users.Messages.List("me").ExecuteAsync();


            foreach (Message? message in listMessagesResponse.Messages)
            {
                _logger.Log(LogLevel.Information, "", message.Id);
                if (message.Id != null)
                {
                    Message messagePart = await _gmailService.Users.Messages
                        .Get("me", message.Id).ExecuteAsync();
                    Console.WriteLine(messagePart.Id);
                    string From = messagePart.Payload.Headers.Where(p => p.Name == "From").First().Value;
                    string Subject = messagePart.Payload.Headers.Where(p => p.Name == "Subject").First().Value;
                    string Date = messagePart.Payload.Headers.Where(p => p.Name == "Date").First().Value;
                    Date = DateTime.ParseExact(Date, "ddd, dd MMM yyyy hh:mm:ss ", CultureInfo.CurrentCulture).ToString("F");


                    messageInfos.Add(new MessageInfo() { Id = message.Id, From = From, Date = Date, To = "", Subject = Subject, IsSelected = false });
                }
            }
            return messageInfos.DistinctBy(p => p.From).ToList();
        }

        public async Task<List<MessageInfo>> deleteMails(string[] senders)
        {
            BatchDeleteMessagesRequest deleteMessagesRequest = new BatchDeleteMessagesRequest();
            deleteMessagesRequest.Ids = new List<string>();
            foreach (string sender in senders)
            {

                messageInfos.Where(p => p.From.Equals(sender)).ToList()
                   .ForEach(p => deleteMessagesRequest.Ids.Add(p.Id));
                var v = await _gmailService.Users.Messages.BatchDelete(deleteMessagesRequest, "me").ExecuteAsync();
                messageInfos.RemoveAll(p => p.From.Equals(sender));

            }
            return messageInfos.DistinctBy(p => p.From).ToList();
        }

    }
}
