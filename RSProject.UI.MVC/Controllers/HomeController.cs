using System;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using RSProject.UI.MVC.Models;

namespace RSProject.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        [HttpGet]
        public ActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactViewModel cvm)
        {
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }

            string message = $"You have received an email from {cvm.Name} with a subject {cvm.Subject}. Please respond to {cvm.Email} with your response to the following message: <br/>{cvm.Message}";

            MailMessage mm = new MailMessage(
                //FROM
                "",
                //TO 
                "",
                //SUBJECT
                cvm.Subject,
                //BODY
                message
                );

            mm.IsBodyHtml = true;
            mm.Priority = MailPriority.High;
            mm.ReplyToList.Add(cvm.Email);

            SmtpClient client = new SmtpClient("mail.");
            client.Credentials = new NetworkCredential("", "");
            client.Port = 8889;

            try
            {
                client.Send(mm);
            }
            catch (Exception ex)
            {
                ViewBag.CustomerMessage = $"Sorry your request could not be completed at this time. Please try again later.<br/>Error Message:<br/>{ex.StackTrace}";
                return View(cvm);
            }

            return View("EmailConfirmation", cvm);
        }
    }
}
