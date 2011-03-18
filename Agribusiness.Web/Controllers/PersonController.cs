﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using Resources;
using UCDArch.Core.PersistanceSupport;
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
        private readonly IMembershipService _membershipService;

        public PersonController(IRepository<Person> personRepository, IRepository<User> userRepository, IPictureService pictureService)
        {
            _personRepository = personRepository;
            _userRepository = userRepository;
            _pictureService = pictureService;

            _membershipService = new AccountMembershipService();
        }

        //
        // GET: /Person/
        public ActionResult Index()
        {
            var personList = _personRepository.Queryable;

            return View(personList);
        }

        public ActionResult Create()
        {
            var viewModel = PersonViewModel.Create(Repository);
            return View(viewModel);
        }

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

        public ActionResult UpdateProfilePicture(int id)
        {
            var person = _personRepository.GetNullableById(id);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", id);
                return this.RedirectToAction(a => a.Index());
            }

            return View(person);
        }

        [HttpPost]
        public ActionResult UpdateProfilePicture(int id, int x, int y, int height, int width)
        {
            var person = _personRepository.GetNullableById(id);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", id);
                return this.RedirectToAction(a => a.Index());
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

        [HttpPost]
        public ActionResult UploadPhoto(int id, HttpPostedFileBase profilepic)
        {
            return View();
        }

        public ActionResult GetProfilePicture(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            if (person.MainProfilePicture != null) return File(person.MainProfilePicture, person.ContentType);

            // load the default image
            var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder.jpg"), FileMode.Open, FileAccess.Read);
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
            var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            return File(img, "image/jpeg");
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
    }
}
