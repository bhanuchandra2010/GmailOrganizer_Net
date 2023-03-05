using GmailOrganizer.Models;
using GmailOrganizer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GmailOrganizer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IMailServices _mailServices;
        public HomeController(ILogger<HomeController> logger, IMailServices mailServices)
        {
            _logger = logger;
            _mailServices = mailServices;
        }


        public async Task<IActionResult> Index()
        {
            List<MessageInfo> lstMessage = await _mailServices.getMails();
            return View(lstMessage);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string[] senders)
        {
            await _mailServices.DeleteMessages(senders);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}