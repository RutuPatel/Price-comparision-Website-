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
    public class searchkeywordController : Controller
    {
       

        public ActionResult Getkeyword()
        {
            SearchKeywordModel moodel = new SearchKeywordModel();
            return View();
        }

        [HttpPost]
        public ActionResult Getkeyword(SearchKeywordModel model)
        {

            return View();
        }
    }
}