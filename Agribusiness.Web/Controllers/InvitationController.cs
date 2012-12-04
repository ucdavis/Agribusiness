using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Invitation class
    /// </summary>
    public class InvitationController : ApplicationController
    {
	    private readonly IRepository<Invitation> _invitationRepository;
        private readonly INotificationService _notificationService;
        private readonly IEventService _eventService;

        public InvitationController(IRepository<Invitation> invitationRepository, INotificationService notificationService, IEventService eventService )
        {
            _invitationRepository = invitationRepository;
            _notificationService = notificationService;
            _eventService = eventService;
        }

        /// <summary>
        /// Invitation list for a seminar
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        public ActionResult Index(int id)
        {
            ViewBag.SeminarId = id;  

            var invitationList = _invitationRepository.Queryable.Where(a => a.Seminar.Id == id);

            return View(invitationList);
        }

        /// <summary>
        /// Add all people from database into invitation list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public RedirectToRouteResult AddAll(int id)
        {
            // load all people in the database
            var site = SiteService.LoadSite(Site);
            var people = site.People;
            var seminar = Repository.OfType<Seminar>().GetNullableById(id);

            if (seminar == null) return this.RedirectToAction<ErrorController>(a => a.Index());

            int count = 0;

            foreach(var person in people)
            {
                var reg = person.GetLatestRegistration();
                var title = reg != null ? reg.Title : string.Empty;
                var firmName = reg != null ? reg.Firm.Name : string.Empty;

                AddToInvitationList(seminar, person, Site, title, firmName);

                count++;
            }

            Message = string.Format("{0} people have been added to the invitation list.", count);
            return this.RedirectToAction(a => a.Index(id));
        }

        /// <summary>
        /// Add a person to the invitation list with checks to make sure duplicates are not inserted
        /// </summary>
        /// <param name="seminar"></param>
        /// <param name="person"></param>
        /// <param name="title"></param>
        /// <param name="firmname"></param>
        private bool AddToInvitationList(Seminar seminar, Person person, string siteId, string title = null, string firmname = null)
        {
            Check.Require(person != null, "person is required.");
            Check.Require(seminar != null, "seminar is required.");

            var invitationList = seminar.Invitations;

            // not yet in the list
            if (!invitationList.Where(a => a.Person == person).Any())
            {
                var invitation = new Invitation(person){Title=title, FirmName = firmname, Seminar = seminar};
                _invitationRepository.EnsurePersistent(invitation);

                _eventService.Invite(person, siteId);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Add a person to the invitation list
        /// </summary>
        /// <returns></returns>
        public ActionResult Create(int personId)
        {
            var person = Repository.OfType<Person>().GetNullableById(personId);
            var seminar = SiteService.GetLatestSeminar(Site);

            if (person == null || seminar == null) return this.RedirectToAction<ErrorController>(a => a.Index());

            var reg = person.GetLatestRegistration();

            var invitation = new Invitation(person) {Seminar = seminar, Title= reg != null ? reg.Title : string.Empty, FirmName = reg != null && reg.Firm != null ? reg.Firm.Name : string.Empty};

            return View(invitation);
        }

        //
        // POST: /Invitation/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(int personId, Invitation invitation)
        {
            var person = Repository.OfType<Person>().GetNullableById(personId);
            var seminar = SiteService.GetLatestSeminar(Site);

            if (person == null || seminar == null) return this.RedirectToAction<ErrorController>(a => a.Index());

            if (AddToInvitationList(seminar, person, Site, invitation.Title, invitation.FirmName))
            {
                Message = "Invitation Created Successfully";

                return this.RedirectToAction<PersonController>(a => a.AdminEdit(person.User.Id, null, true));
            }
            
            Message = "Person already on invitation list.";

            invitation.Person = person;
            invitation.Seminar = seminar;

            return View(invitation);
        }

        //
        // GET: /Invitation/Edit/5
        public ActionResult Edit(int id)
        {
            var invitation = _invitationRepository.GetNullableById(id);

            if (invitation == null) return RedirectToAction("Index");

            return View(invitation);
        }

        //
        // POST: /Invitation/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Invitation invitation)
        {
            var invitationToEdit = _invitationRepository.GetNullableById(id);

            if (invitationToEdit == null) return RedirectToAction("Index");

            TransferValues(invitation, invitationToEdit);

            if (ModelState.IsValid)
            {
                _invitationRepository.EnsurePersistent(invitationToEdit);

                Message = "Invitation Edited Successfully";

                return RedirectToAction("Index");
            }
            
            return View(invitationToEdit);
        }
        
        //
        // GET: /Invitation/Delete/5 
        public ActionResult Delete(int id)
        {
			var invitation = _invitationRepository.GetNullableById(id);

            if (invitation == null) return RedirectToAction("Index");

            return View(invitation);
        }

        //
        // POST: /Invitation/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, Invitation invitation)
        {
			var invitationToDelete = _invitationRepository.GetNullableById(id);

            if (invitationToDelete == null) return RedirectToAction("Index");

            var person = invitationToDelete.Person;
            var seminar = invitationToDelete.Seminar;
            _notificationService.RemoveFromMailingList(seminar, person, MailingLists.Invitation);

            _invitationRepository.Remove(invitationToDelete);

            Message = "Invitation Removed Successfully";

            return RedirectToAction("Index", new {id=seminar.Id});
        }
        
        /// <summary>
        /// Transfer editable values from source to destination.  Use of AutoMapper is recommended
        /// </summary>
        private static void TransferValues(Invitation source, Invitation destination)
        {
            destination.Title = source.Title;
            destination.FirmName = source.FirmName;
        }

    }

	
	/// <summary>
    /// ViewModel for the Invitation class
    /// </summary>
    public class InvitationViewModel
	{
		public Invitation Invitation { get; set; }
 
		public static InvitationViewModel Create(IRepository repository, Invitation invitation = null)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new InvitationViewModel {Invitation = invitation ?? new Invitation()};
 
			return viewModel;
		}
	}
}
