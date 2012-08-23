using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using Agribusiness.WS;
using Resources;
using UCDArch.Core.PersistanceSupport;
using MvcContrib;
using UCDArch.Web.Helpers;

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
        private readonly IRepository<Firm> _firmRepository;
        private readonly IPersonService _personService;
        private readonly IRegistrationService _registrationService;
        private readonly IFirmService _firmService;
        private readonly Agribusiness.Web.Services.INotificationService _notificationService;

        public AttendeeController(IRepository<Seminar> seminarRespository, IRepositoryWithTypedId<User, Guid> userRepository, IRepository<SeminarPerson> seminarPersonRepository
                                , IRepository<Person> personRepository, IRepository<Firm> firmRepository
                                , IPersonService personService, IRegistrationService registrationService, IFirmService firmService, Agribusiness.Web.Services.INotificationService notificationService)
        {
            _seminarRespository = seminarRespository;
            _userRepository = userRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _personRepository = personRepository;
            _firmRepository = firmRepository;
            _personService = personService;
            _registrationService = registrationService;
            _firmService = firmService;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Attendee List for a seminar
        /// </summary>
        /// <param name="id">seminar id</param>
        /// <returns></returns>
        [UserOnly]
        public ActionResult Index(int id)
        {
            var seminar = _seminarRespository.GetNullableById(id);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction("Index", "Seminar");
            }

            var viewModel = AttendeeListViewModel.Create(seminar, _personService, Site);
            return View(viewModel);
        }

        /// <summary>
        /// Update all records of registration with data from crp
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        [UserOnly]
        [HttpPost]
        public ActionResult UpdateAllRegistrations(int id)
        {
            var seminar = _seminarRespository.GetNullableById(id);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            // make the remote call
            var result = _registrationService.RefreshAllRegistration(seminar.RegistrationId.Value);

            // load all of the seminar people
            var seminarPeople = seminar.SeminarPeople;

            foreach (var sp in seminarPeople)
            {
                var reg = result.Where(a => a.ReferenceId == sp.ReferenceId).FirstOrDefault();

                if (reg != null)
                {
                    sp.TransactionId = reg.TransactionId;
                    sp.Paid = reg.Paid;

                    // remove from the payment reminder mailing lists
                    if (sp.Paid)
                    {
                        _notificationService.RemoveFromMailingList(sp.Seminar, sp.Person, MailingLists.PaymentReminder);
                        _notificationService.RemoveFromMailingList(sp.Seminar, sp.Person, MailingLists.Registered);
                        _notificationService.AddToMailingList(sp.Seminar, sp.Person, MailingLists.Attending);
                    }

                    _seminarPersonRepository.EnsurePersistent(sp);
                }
            }

            Message = "Registration status' for all attendees have been updated.";
            return this.RedirectToAction(a => a.Index(id));
        }

        /// <summary>
        /// Add an attendee to a seminar
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        [UserOnly]
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

        /*
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
        */

        [UserOnly]
        public ActionResult AddConfirm(int id, int personId)
        {
            var seminar = _seminarRespository.GetNullableById(id);
            
            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "person", personId);
                return this.RedirectToAction(a => a.Add(id));
            }
            
            var seminarPerson = person.GetLatestRegistration();

            if (seminarPerson != null && seminarPerson.Seminar == seminar)
            {
                Message = string.Format("{0} is already part of this seminar and cannot be added.", person.FullName);
                return this.RedirectToAction(a => a.Add(id));
            }

            var viewModel = AddConfirmViewModel.Create(Repository, _firmService, seminar, person, seminarPerson, seminarPerson != null ? seminarPerson.Firm : null);
            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult AddConfirm(int id, int personId, SeminarPerson seminarPerson, Firm firm)
        {
            var seminar = _seminarRespository.GetNullableById(id);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var person = _personRepository.GetNullableById(personId);

            if (person == null)
            {
                Message = string.Format(Messages.NotFound, "person", personId);
                return this.RedirectToAction(a => a.Add(id));
            }

            if (firm.Id > 0)
            {
                seminarPerson.Firm = _firmRepository.GetNullableById(firm.Id);
            }
            else if (firm.Id == 0)
            {
                seminarPerson.Firm = firm;
            }

            // fill in the values
            seminarPerson.Person = person;
            seminarPerson.Seminar = seminar;

            ModelState.Clear();
            seminarPerson.TransferValidationMessagesTo(ModelState);

            if (seminarPerson.Firm != null)
            {
                seminarPerson.Firm.TransferValidationMessagesTo(ModelState);    
            }

            if (ModelState.IsValid)
            {
                _seminarPersonRepository.EnsurePersistent(seminarPerson);
                Message = string.Format("{0} has been added to the {1} seminar.", person.FullName, seminar.Year);

                _notificationService.AddToMailingList(seminar, person, MailingLists.Registered);

                // add user to the reminder emails
                _notificationService.AddToMailingList(seminar, person, MailingLists.PaymentReminder);
                _notificationService.AddToMailingList(seminar, person, MailingLists.HotelReminder);

                if (string.IsNullOrWhiteSpace(person.Biography)) _notificationService.AddToMailingList(seminar, person, MailingLists.BioReminder);
                if (person.OriginalPicture == null) _notificationService.AddToMailingList(seminar, person, MailingLists.PhotoReminder);

                return this.RedirectToAction(a => a.Add(id));
            }

            var viewModel = AddConfirmViewModel.Create(Repository, _firmService, seminar, person, seminarPerson, seminarPerson.Firm);
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
        /// <param name="seminarId"></param>
        /// <returns></returns>
        [MembershipUserOnly]
        public ActionResult Profile(int id, int seminarId)
        {
            var person = _personRepository.GetNullableById(id);

            var seminar = _seminarRespository.GetNullableById(seminarId);

            if (person == null || seminar == null)
            {
                Message = string.Format(Messages.NotFound, "person/seminar", id);
                return this.RedirectToAction(a => a.BySeminar(seminarId));
            }

            // make sure the person should have access
            if (!_personService.HasAccess(person, seminar)) return this.RedirectToAction<ErrorController>(a => a.NotAuthorized());

            var displayPerson = _personService.GetDisplayPerson(person);

            TempData["SeminarId"] = seminarId;

            return View(displayPerson);
        }
        #endregion
    }
}
