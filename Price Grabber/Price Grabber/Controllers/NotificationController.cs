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
using System.Collections.Generic;

namespace Price_Grabber.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        PricegrabberEntities db = new PricegrabberEntities();        
        
        public ActionResult Notification(string ProductName, string  ProductPrice)
        {
            
            var model = new NotificationHomeModel();
            model.notification.ProductName = ProductName;
            model.notification.Price = ProductPrice;
            var notifications = db.Notifications.Where(x=>x.Email == User.Identity.Name);
            foreach (var notification in notifications)
            {
                model.NotificationList.Add(new NotificationModel()
                {
                    ProductName = notification.ProductName,
                    Email = notification.Email,
                    Id = notification.Id,
                    Price = notification.Price.ToString()
            });
            }        
                return View(model);
        }

        [HttpPost]       
        public ActionResult Notification(NotificationHomeModel model)
        {
            
            Notification NotificationDetails = new Notification();
            NotificationDetails.Email = User.Identity.Name;
            NotificationDetails.Price = Convert.ToDecimal(model.notification.Price);            
            NotificationDetails.ProductName = model.notification.ProductName;
            db.Notifications.Add(NotificationDetails);
            db.SaveChanges();
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress("info.pricegrabber@gmail.com");
            mm.Subject = "Notification Set for Product";
            mm.Body = string.Format("<html><body><h2>Price Grabber</h2><p>New Notification set for following product.</p><br/><h3>Product</h3></body></html>" + "<html><body><h2>Title</h2></body></html>" + model.notification.ProductName + "<html><body><h2>Set Price</h2></body></html>" + model.notification.Price);
            mm.To.Add(new MailAddress(User.Identity.Name));
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            // smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("info.pricegrabber@gmail.com", "high@low123");
            //smtp.UseDefaultCredentials = true;
            smtp.UseDefaultCredentials = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = NetworkCred;
            smtp.Timeout = 10000;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mm);
            return RedirectToAction("Notification", "Notification");           
        }
    }
}