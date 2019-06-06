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
using System.Web.Security;

namespace Price_Grabber.Controllers
{
   
   
    [Authorize]
    public class AccountController : Controller
    {
        PricegrabberEntities db = new PricegrabberEntities();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            { 
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value;    
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }


        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var existingUser = db.Users.Where(x => x.Email == model.Email);
                if (existingUser.Any())
                {
                    ModelState.AddModelError("Email", "Username already exist");
                }
                else
                {

                    User UserDetails = new User();
                    UserDetails.Email = model.Email;
                    UserDetails.Password = model.Password;
                    db.Users.Add(UserDetails);
                    db.SaveChanges();
                    MailMessage mm = new MailMessage();
                    mm.From = new MailAddress("info.pricegrabber@gmail.com");
                    mm.Subject = "Account SignUP Confirmation";
                    mm.Body= string.Format("<html><body><h2>Price Grabber</h2><p>New SignUP for Price Grabber.You received this message because now  you are registered for info.pricegrabber@gmail.com. Your Account is successfully created</p><br/></body></html>");
                  
                    mm.To.Add(new MailAddress(model.Email));
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

                    return RedirectToAction("LogIn", "Account");
                }



                //var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                //var result = await UserManager.CreateAsync(user, model.Password);
                //if (result.Succeeded)
                //{
                //    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                //    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                //    // Send an email with this link
                //    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                //    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");


                //}
                //AddErrors(result);


                // If we got this far, something failed, redisplay form

            }
            return View();
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

       
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                var loginCheck = db.Users.Where(x => x.Email.Equals(model.Email) && x.Password.Equals(model.Password)).FirstOrDefault();
                if (loginCheck != null)
                {
                    FormsAuthentication.SetAuthCookie(loginCheck.Email, false);
                    Session["Email"] = loginCheck.Email;
                    MailMessage mm = new MailMessage();
                    mm.From = new MailAddress("info.pricegrabber@gmail.com");
                    mm.Subject = "Account SignIn Alert";
                    mm.Body = string.Format("<html><body><h2>Price Grabber</h2><p>New SignIn From Windows.Please Review you login</p><br/></body></html>");
                    mm.To.Add(new MailAddress(model.Email));
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
                    return RedirectToAction("Index", "Home");

                }
                else
                {                
                    ModelState.AddModelError("Email", "Username or Password Invalid");                                 
                }
            }
            return View();         
           
           
        }
        
        //if (!ModelState.IsValid)
        //{
        //    return View(model);
        //}

        //// This doesn't count login failures towards account lockout
        //// To enable password failures to trigger account lockout, change to shouldLockout: true
        
        //switch (result)
        //{
        //    case SignInStatus.Success:
        //        return RedirectToLocal(returnUrl);
        //    case SignInStatus.LockedOut:
        //        return View("Lockout");
        //    case SignInStatus.RequiresVerification:
        //        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
        //    case SignInStatus.Failure:
        //    default:
        //        ModelState.AddModelError("", "Invalid login attempt.");
        //        return View(model);
        //}
    

        
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }    

        
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == model.Email);
            //var UserId = db.Users.FirstOrDefault(x => x.Id == email.Id);
            if (user != null)
            {

                Guid id = Guid.NewGuid();
                user.ForgotPasswordID = id.ToString();               
                db.SaveChanges();
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("info.pricegrabber@gmail.com");
                mm.Subject = "Password Reset link";
                mm.Body = string.Format("<html><body><h2>Price Grabber</h2><p>For Reset your Account Password click below button</p><br/><a style='font-size: 16px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; background-color: #EB7035; border-top: 12px solid #EB7035; border-bottom: 12px solid #EB7035; border-right: 18px solid #EB7035; border-left: 18px solid #EB7035; display: inline-block;' href='http://localhost:59812/Account/ResetPassword?userhash={0}' target='_blank'>Reset Password</a></body></html>",id.ToString());  
             // mm.Body = "@Html.ActionLink("reset""ResetPassword")";
                mm.To.Add(new MailAddress(model.Email));
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                //smtp.Host = "smtp.gmail.com";
                // smtp.EnableSsl = true;
                //NetworkCredential NetworkCred = new NetworkCredential("info.pricegrabber@gmail.com", "high@low123");
                //smtp.UseDefaultCredentials = true;
                //smtp.UseDefaultCredentials = true;
                //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtp.Credentials = NetworkCred;
                //smtp.Timeout = 10000;
                //smtp.Port = 587;
                //smtp.EnableSsl = true;
                smtp.Send(mm);

                return View("ForgotPasswordConfirmation");
            }
            else
            {
                ModelState.AddModelError("Email", "Please provide the correct email address!");
            }
            return View();

            //if (ModelState.IsValid)
            //{
            //    var user = await UserManager.FindByNameAsync(model.Email);
            //    if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            //    {
            //        // Don't reveal that the user does not exist or is not confirmed
            //        return View("ForgotPasswordConfirmation");
            //    }

            //    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //    // Send an email with this link
            //    // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            //    // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
            //    // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
            //    // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            //}

            // If we got this far, something failed, redisplay form

        }

        
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string userhash)
        {
            var userModel = new ResetPasswordViewModel();
            if (!string.IsNullOrEmpty(userhash))
            {
                /* find the user using the userhash */
                var user = db.Users.FirstOrDefault(x => x.ForgotPasswordID == userhash);
                if (user != null)
                {
                    userModel.Email = user.Email;

                }
               
            }
            userModel.Code = userhash;
            return View(userModel);
        }

        
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult  ResetPassword(ResetPasswordViewModel model)
        {
           
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = db.Users.FirstOrDefault(x => x.ForgotPasswordID == model.Code );
            if(user!=null)
            {
              
                user.Password = model.Password;
                db.SaveChanges();
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("info.pricegrabber@gmail.com");
                mm.Subject = "Password Reset Successfully";
                mm.Body = string.Format("<html><body><h2>Price Grabber</h2><p>Your password has been succesfully reset click below to to login</p><br/><a style='font-size: 16px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; text-decoration: none; border-radius: 3px; -webkit-border-radius: 3px; -moz-border-radius: 3px; background-color: #EB7035; border-top: 12px solid #EB7035; border-bottom: 12px solid #EB7035; border-right: 18px solid #EB7035; border-left: 18px solid #EB7035; display: inline-block;' href='http://localhost:59812/Account/Login' target='_blank'> LOGIN </a></body></html>");
                mm.To.Add(new MailAddress(model.Email));
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Timeout = 10000;               
                smtp.Send(mm);
                return RedirectToAction("ResetPasswordConfirmation", "Account");               
            }
            else
            {
                ModelState.AddModelError("Email", "Please provide the correct email address!");
            }
            //var user = await UserManager.FindByNameAsync(model.Email);
            //if (user == null)
            //{
            //    // Don't reveal that the user does not exist
            //    return RedirectToAction("ResetPasswordConfirmation", "Account");
            //}
            //var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);


            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        // [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
           // AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}