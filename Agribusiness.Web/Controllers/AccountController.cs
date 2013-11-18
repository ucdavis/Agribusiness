using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using MvcContrib;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Authentication;

namespace Agribusiness.Web.Controllers
{
    public class AccountController : ApplicationController
    {
        private readonly IRepositoryWithTypedId<User, Guid> _userRepository;
        private readonly IRepositoryWithTypedId<Agribusiness.Core.Domain.Membership, Guid> _membershipRepository;

        public AccountController(IRepositoryWithTypedId<User, Guid> userRepository, IRepositoryWithTypedId<Agribusiness.Core.Domain.Membership, Guid> membershipRepository)
        {
            _userRepository = userRepository;
            _membershipRepository = membershipRepository;
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
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.Site = Site;

            if (!membershipLogon) return this.RedirectToAction(a => a.CasLogon(returnUrl));

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl, string site)
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
                        return RedirectToAction("Index", "Authorized", new {site});
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
            ViewBag.Site = Site;
            ViewBag.PasswordLength = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model, string site)
        {
            ViewBag.Site = site;
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess", new {site});
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
            ViewBag.Site = Site;
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(string userName, string email, string site)
        {
            ViewBag.Site = site;
            // lookup by email
            if (string.IsNullOrEmpty(userName) && !string.IsNullOrWhiteSpace(email))
            {
                var membership = _membershipRepository.Queryable.Where(a => a.LoweredEmail == email.ToLower()).FirstOrDefault();
                if (membership != null && membership.User != null)
                {
                    userName = membership.User.UserName;
                }
            }

            if (MembershipService.ResetPassword(userName))
            {
                Message = "Your password has been reset, please check your email";
                //return this.RedirectToAction<AccountController>(a => a.LogOn(null, true));
                return RedirectToAction("LogOn", "Account", new {membershipLogon = true, site = site});
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
