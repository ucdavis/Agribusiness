using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Authentication;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    public class AccountController : ApplicationController
    {
        private readonly IRepository<User> _userRepository;

        public AccountController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn(string returnUrl, bool membershipLogon = true)
        {
            if (!membershipLogon) return this.RedirectToAction(a => a.CasLogon(returnUrl));

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************
        [UserOnly]
        public ActionResult Register(string email)
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            var model = new RegisterModel(){Email = email, UserName = email};
            return View(model);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            // clear validation because username is not provided
            ModelState.Clear();

            // set hte username to the email
            model.UserName = model.Email.ToLower();

            var ctx = new ValidationContext(model, null, null);
            var rst = new List<ValidationResult>();
            Validator.TryValidateObject(model, ctx, rst);
            foreach (var a in rst) { ModelState.AddModelError(a.MemberNames.First(), a.ErrorMessage); }

            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "InformationRequest");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View(model);
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string userName, string email)
        {
            // lookup by email
            if (string.IsNullOrEmpty(userName) && !string.IsNullOrWhiteSpace(email))
            {
                var user = _userRepository.Queryable.Where(a => a.Membership.Email == email).FirstOrDefault();

                if (user != null) userName = user.LoweredUserName;
            }

            if (MembershipService.ResetPassword(userName))
            {
                Message = "Your password has been reset, please check your email";
                return this.RedirectToAction<HomeController>(a => a.Index());
            }

            ModelState.AddModelError("", "Invalid account name provided.");

            return View();
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        // **************************************
        // Cas Logon
        // **************************************
        public ActionResult CasLogon(string returnUrl)
        {
            string resultUrl = CASHelper.Login();   // do cas logon

            if (resultUrl != null)
            {
                return Redirect(resultUrl);
            }

            return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
        }

    }
}
