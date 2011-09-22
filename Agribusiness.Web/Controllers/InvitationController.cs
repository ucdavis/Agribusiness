using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
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

        public InvitationController(IRepository<Invitation> invitationRepository)
        {
            _invitationRepository = invitationRepository;
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
            var people = Repository.OfType<Person>().GetAll();
            var seminar = Repository.OfType<Seminar>().GetNullableById(id);

            if (seminar == null) return this.RedirectToAction<ErrorController>(a => a.Index());

            int count = 0;

            foreach(var person in people)
            {
                var reg = person.GetLatestRegistration();
                var title = reg != null ? reg.Title : string.Empty;
                var firmName = reg != null ? reg.Firm.Name : string.Empty;

                AddToInvitationList(seminar, person, title, firmName);

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
        private void AddToInvitationList(Seminar seminar, Person person, string title = null, string firmname = null)
        {
            Check.Require(person != null, "person is required.");
            Check.Require(seminar != null, "seminar is required.");

            var invitationList = seminar.Invitations;

            // not yet in the list
            if (!invitationList.Where(a => a.Person == person).Any())
            {
                var invitation = new Invitation(person){Title=title, FirmName = firmname, Seminar = seminar};
                _invitationRepository.EnsurePersistent(invitation);
            }
        }

        /// <summary>
        /// Add a person to the invitation list
        /// </summary>
        /// <returns></returns>
        public ActionResult Create(int personId, int seminarId)
        {
            var person = Repository.OfType<Person>().GetNullableById(personId);
            var seminar = Repository.OfType<Seminar>().GetNullableById(seminarId);

            if (person == null || seminar == null) return this.RedirectToAction<ErrorController>(a => a.Index());

            var invitation = new Invitation(person) {Seminar = seminar};

            return View(invitation);
        }

        //
        // POST: /Invitation/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Invitation invitation)
        {
            var invitationToCreate = new Invitation();

            TransferValues(invitation, invitationToCreate);

            if (ModelState.IsValid)
            {
                _invitationRepository.EnsurePersistent(invitationToCreate);

                Message = "Invitation Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
                var viewModel = InvitationViewModel.Create(Repository);
                viewModel.Invitation = invitation;

                return View(viewModel);
            }
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

            _invitationRepository.Remove(invitationToDelete);

            Message = "Invitation Removed Successfully";

            return RedirectToAction("Index");
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
