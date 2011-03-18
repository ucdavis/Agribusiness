using System.Collections.Generic;
using System.IO;
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
        private readonly IPictureService _pictureService;

        public PersonController(IRepository<Person> personRepository, IPictureService pictureService)
        {
            _personRepository = personRepository;
            _pictureService = pictureService;
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

            SetAddresses(person, personEditModel.Addresses);

            if (profilepic != null)
            {
                // read the file
                var reader = new BinaryReader(profilepic.InputStream);
                var data = reader.ReadBytes(profilepic.ContentLength);
                person.OriginalPicture = data;
            }

            ModelState.Clear();
            person.TransferValidationMessagesTo(ModelState);

            //if (ModelState.IsValid)
            //{
            //    _personRepository.EnsurePersistent(person);
            //    Message = string.Format(Messages.Saved, "Person");
            //    return this.RedirectToAction(a => a.UpdateProfilePicture(person.Id));
            //}

            Message = "Test";

            var viewModel = PersonViewModel.Create(Repository, person);
            return View(viewModel);
        }

        public ActionResult UpdateProfilePicture(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateProfilePicture(int id, int x, int y)
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadPhoto(int id, HttpPostedFileBase profilepic)
        {
            return View();
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
