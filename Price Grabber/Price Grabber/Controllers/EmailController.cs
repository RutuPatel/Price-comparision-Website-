using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Price_Grabber.Models;
using Nitin.Sms.Api;
using System.Net.Mail;
using System.Net;


namespace Price_Grabber.Controllers
{
    public class EmailController : Controller
    {
        public object ClientScript { get; private set; }

        public ActionResult SendEmail()
        {
            EmailModel moodel = new EmailModel();
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(EmailModel model, LoginViewModel Loginmodel)
        {
            try
            {
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("patelrutu1203@gmail.com");
                mm.Subject = "Notification mail";
                mm.Body ="hello";
                mm.To.Add(new MailAddress(model.To));
                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                // smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential(model.From, model.Password);
                //smtp.UseDefaultCredentials = true;
                smtp.UseDefaultCredentials = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = NetworkCred;
                smtp.Timeout = 10000;
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(mm);         



            }
            catch (Exception e)
            {

            }


            return View();
        }
        }
    }
