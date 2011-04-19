﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Person class
    /// </summary>
    public class PersonController : ApplicationController
    {
	    private readonly IRepository<Person> _personRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IPictureService _pictureService;
        private readonly IPersonService _personService;
        private readonly IFirmService _firmService;
        private readonly IMembershipService _membershipService;

        public PersonController(IRepository<Person> personRepository, IRepository<User> userRepository, IPictureService pictureService, IPersonService personService, IFirmService firmService)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _pictureService = pictureService;
            _personService = personService;
            _firmService = firmService;

            _membershipService = new AccountMembershipService();
        }

        //
        // GET: /Person/
        public ActionResult Index()
        {
            var viewModel = PersonListViewModel.Create(_personRepository, _personService);
            return View(viewModel);
        }

        public ActionResult Details(int id)
        {
            var person = _personRepository.GetNullableById(id);

            if (person == null)
            {
                Message = "Could not locate person.";
                return this.RedirectToAction(a => a.Index());
            }

            var displayPerson = _personService.GetDisplayPerson(person);
            return View(displayPerson);
        }

        #region Administration Functions
        [UserOnly]
        public ActionResult Create()
        {
            var viewModel = PersonViewModel.Create(Repository);
            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Create(PersonEditModel personEditModel, HttpPostedFileBase profilepic)
        {
            var person = personEditModel.Person;

            // create an account
            var createStatus =  _membershipService.CreateUser(personEditModel.Email
                                          , Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 10)
                                          , personEditModel.Email);

            var user = _userRepository.Queryable.Where(a => a.UserName == personEditModel.Email).FirstOrDefault();
            person.User = user;

            SetAddresses(person, personEditModel.Addresses);

            if (profilepic != null)
            {
                // read the file
                var reader = new BinaryReader(profilepic.InputStream);
                person.OriginalPicture = reader.ReadBytes(profilepic.ContentLength);
                person.ContentType = profilepic.ContentType;
            }

            ModelState.Clear();
            person.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _personRepository.EnsurePersistent(person);
                Message = string.Format(Messages.Saved, "Person");
                return this.RedirectToAction(a => a.UpdateProfilePicture(person.Id));
            }

            var viewModel = PersonViewModel.Create(Repository, person);
            viewModel.Addresses = personEditModel.Addresses;
            viewModel.Email = personEditModel.Email;
            return View(viewModel);
        }
        [UserOnly]
        [HttpPost]
        public ActionResult UploadPhoto(int id, HttpPostedFileBase profilepic)
        {
            return View();
        }
        #endregion

        #region Profile Editing Functions
        /// <summary>
        /// Attendee's page to update their own profile
        /// </summary>
        /// <param name="id">Person Id for admin editing</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(int? id)
        {
            User user;

            // admin is trying to edit, authorize them
            if (id.HasValue)
            {
                user = Repository.OfType<User>().GetNullableById(id.Value);

                // current user must be in User role
                if (!Roles.IsUserInRole(RoleNames.User))
                {
                    return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
                }
            }
            else
            {
                user = Repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower()).FirstOrDefault();    
            }

            if (user == null)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }

            var person = user.Person;

            var viewModel = PersonViewModel.Create(Repository, person);
            viewModel.Email = CurrentUser.Identity.Name.ToLower();
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Edit(int? id, PersonEditModel personEditModel, HttpPostedFileBase profilepic)
        {
            User user = null;

            // admin is trying to edit, authorize them
            if (id.HasValue)
            {
                // current user must be in User role
                if (Roles.IsUserInRole(RoleNames.User))
                {
                    user = Repository.OfType<User>().GetNullableById(id.Value);
                }
            }
            else
            {
                user = Repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower()).FirstOrDefault();
            }

            if (user == null)
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }

            var person = SetPerson(personEditModel, ModelState, user.Person, profilepic);

            if (ModelState.IsValid)
            {
                _personRepository.EnsurePersistent(person);
                Message = string.Format(Messages.Saved, "Person");

                // send to crop photo if one was uploaded
                if (profilepic != null) return this.RedirectToAction(a => a.UpdateProfilePicture(person.Id));
            }

            var viewModel = PersonViewModel.Create(Repository, person);
            viewModel.Email = user.LoweredUserName;
            return View(viewModel);
        }

        #endregion

        #region Profile Picture Actions
        [Authorize]
        public ActionResult UpdateProfilePicture(int id)
        {
            var person = _personRepository.GetNullableById(id);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", id);
                return this.RedirectToAction(a => a.Index());
            }

            // validate this is the person or is a person in user role
            if (person.User.LoweredUserName != CurrentUser.Identity.Name.ToLower() && !Roles.IsUserInRole(RoleNames.User))
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }

            return View(person);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateProfilePicture(int id, int x, int y, int height, int width)
        {
            var person = _personRepository.GetNullableById(id);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", id);
                return this.RedirectToAction(a => a.Index());
            }

            // validate this is the person or is a person in user role
            if (person.User.LoweredUserName != CurrentUser.Identity.Name.ToLower() && !Roles.IsUserInRole(RoleNames.User))
            {
                return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());
            }
            
            // crop the image
            var cropped = _pictureService.Crop(person.OriginalPicture, x, y, width, height);

            // get the main profile picture
            person.MainProfilePicture = _pictureService.MakeMainProfile(cropped);

            // get the thumbnail
            person.ThumbnailPicture = _pictureService.MakeThumbnail(cropped);

            person.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                Message = string.Format(Messages.Saved, "Person");
                _personRepository.EnsurePersistent(person);
                return this.RedirectToAction(a => a.Index());
            }

            return View(person);
        }

        [Authorize]
        public ActionResult GetOriginalPicture(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            if (person.OriginalPicture != null) return File(person.OriginalPicture, person.ContentType);

            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profilepicplaceholder.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/jpeg");
        }

        public ActionResult GetProfilePicture(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            if (person.MainProfilePicture != null) return File(person.MainProfilePicture, person.ContentType);

            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profilepicplaceholder.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/jpeg");
        }

        public ActionResult GetThumbnail(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            if (person.MainProfilePicture != null) return File(person.ThumbnailPicture, person.ContentType);

            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder_thumb.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/jpeg");
        }
        #endregion

        #region Private Helpers
        private Person SetPerson(PersonEditModel personEditModel, ModelStateDictionary modelState, Person person = null, HttpPostedFileBase profilePic = null)
        {
            person = person ?? personEditModel.Person;

            // copy all the fields
            Mapper.Map(personEditModel, person);

            // remove the blank address
            var remove = personEditModel.Addresses.Where(a => !a.HasAddress()).ToList();
            foreach (var a in remove) personEditModel.Addresses.Remove(a);

            // update/add updated addresses
            foreach (var addr in personEditModel.Addresses)
            {
                var type = addr.AddressType;
                var origAddress = person.Addresses.Where(a => a.AddressType == type).FirstOrDefault();
                
                // address was entered
                if (addr.HasAddress())
                {
                    // person did not have this address in the first place, add it in
                    if (origAddress == null)
                    {
                        person.AddAddress(addr);
                    }
                    // update existing address
                    else
                    {
                        Mapper.Map(addr, origAddress);    
                    }
                }
                // address was blanked out/removed
                else
                {
                    if (origAddress != null) person.Addresses.Remove(origAddress);
                }
            }

            // deal with the image
            if (profilePic != null)
            {
                // blank out existing image files
                person.OriginalPicture = null;
                person.MainProfilePicture = null;
                person.ThumbnailPicture = null;

                // read the file and set the original picture
                var reader = new BinaryReader(profilePic.InputStream);
                person.OriginalPicture = reader.ReadBytes(profilePic.ContentLength);
                person.ContentType = profilePic.ContentType;                
            }

            // run the validation
            modelState.Clear();
            person.TransferValidationMessagesTo(modelState);

            return person;
        }

        private void SetAddresses(Person person, IEnumerable<Address> addresses)
        {
            foreach (var a in addresses)
            {
                if (a.HasAddress())
                {
                    person.AddAddress(a);
                }
            }
        }
        #endregion
    }
}
