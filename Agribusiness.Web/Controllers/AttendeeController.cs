using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Attendee class
    /// </summary>
    [Authorize]
    public class AttendeeController : ApplicationController
    {
        private readonly IRepository<Seminar> _seminarRespository;
        private readonly IRepositoryWithTypedId<User, Guid> _userRepository;
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IPersonService _personService;

        public AttendeeController(IRepository<Seminar> seminarRespository, IRepositoryWithTypedId<User, Guid> userRepository, IRepository<SeminarPerson> seminarPersonRepository, IRepository<Person> personRepository, IPersonService personService)
        {
            _seminarRespository = seminarRespository;
            _userRepository = userRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _personRepository = personRepository;
            _personService = personService;
        }

        /// <summary>
        /// Attendee List for a seminar
        /// </summary>
        /// <param name="id">seminar id</param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            var seminar = _seminarRespository.GetNullableById(id);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction("Index", "Seminar");
            }

            var viewModel = AttendeeListViewModel.Create(seminar, _personService);
            return View(viewModel);
        }

        /// <summary>
        /// Add an attendee to a seminar
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        public ActionResult Add(int id)
        {
            var seminar = _seminarRespository.GetNullableById(id);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var viewModel = AddAttendeeViewModel.Create(Repository, _personService, seminar);
            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Add(int id, int personId)
        {
            var seminar = _seminarRespository.GetNullableById(id);
            var person = _personRepository.GetNullableById(personId);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "Person", personId);
                return this.RedirectToAction(a => a.Index(id));
            }

            if (seminar.SeminarPeople.Where(a => a.Person.Id == person.Id).Any())
            {
                ModelState.AddModelError("Attendee", string.Format("{0} is already an attendee.", person.FullName));
            }

            // create a new seminar person
            var seminarPerson = new SeminarPerson(seminar, person);

            if (ModelState.IsValid)
            {
                _seminarPersonRepository.EnsurePersistent(seminarPerson);

                Message = string.Format("{0} has been added as an attendee.", person.FullName);
                return this.RedirectToAction(a => a.Index(id));
            }

            var viewModel = AddAttendeeViewModel.Create(Repository, _personService, seminar, person.Id);
            return View(viewModel);
        }

        #region Membership Users
        /// <summary>
        /// List of attendees by seminar
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        [MembershipUserOnly]
        public ActionResult BySeminar(int id)
        {
            var seminar = _seminarRespository.GetNullableById(id);

            // get the user's seminar person id
            var user = _userRepository.Queryable.Where(a => a.LoweredUserName == CurrentUser.Identity.Name.ToLower()).SingleOrDefault();
            if (user == null) return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());

            var person = user.Person;
            var latestReg = person.GetLatestRegistration();

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.MySeminar(latestReg.Id));
            }

            // validate seminar access
            if (!_personService.HasAccess(person, seminar)) return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());

            ViewBag.seminarPersonId = latestReg.Id;

            return View(_personService.GetDisplayPeopleForSeminar(seminar.Id));
        }

        /// <summary>
        /// Profile page for attendees to view
        /// </summary>
        /// <param name="id">Person Id</param>
        /// <returns></returns>
        [MembershipUserOnly]
        public ActionResult Profile(int id)
        {
            return View();
        }
        #endregion
    }
}
