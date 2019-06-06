using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Price_Grabber.Helpers;

namespace Price_Grabber.Models
{
  
    public class NotificationHomeModel
    {
    public NotificationModel notification { get; set; }
        public List<NotificationModel> NotificationList { get; set; }

        public NotificationHomeModel()
        {
            notification = new NotificationModel();
            NotificationList = new List<NotificationModel>();
        }
    }


}