using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using MvcContrib;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the InformationRequest class
    /// </summary>
    public class InformationRequestController : ApplicationController
    {
	    private readonly IRepository<InformationRequest> _informationrequestRepository;
        private readonly IRepository<InformationRequestNote> _informationRequestNoteRepository;

        public InformationRequestController(IRepository<InformationRequest> informationrequestRepository, IRepository<InformationRequestNote> informationRequestNoteRepository)
        {
            _informationrequestRepository = informationrequestRepository;
            _informationRequestNoteRepository = informationRequestNoteRepository;
        }

        //
        // GET: /InformationRequest/
        public ActionResult Index()
        {
            var informationrequestList = _informationrequestRepository.Queryable.OrderBy(a=>a.Responded);

            return View(informationrequestList);
        }

        public ActionResult Edit(int id)
        {
            var informationRequest = _informationrequestRepository.GetNullableById(id);

            if (informationRequest == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            return View(informationRequest);
        }

        [HttpPost]
        public ActionResult Edit(int id, InformationRequest informationRequest)
        {
            var editInformationRequest = _informationrequestRepository.GetNullableById(id);

            if (editInformationRequest == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            Mapper.Map(informationRequest, editInformationRequest);

            ModelState.Clear();
            editInformationRequest.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _informationrequestRepository.EnsurePersistent(editInformationRequest);
                Message = string.Format(Messages.Saved, "Information request");
            }

            return View(editInformationRequest);
        }

        public ActionResult AddNote(int id)
        {
            var informationRequest = _informationrequestRepository.GetNullableById(id);

            if (informationRequest == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            return View(new InformationRequestNote(informationRequest, string.Empty, CurrentUser.Identity.Name));
        }

        [HttpPost]
        public ActionResult AddNote(int id, InformationRequestNote informationRequestNote)
        {
            var informationRequest = _informationrequestRepository.GetNullableById(id);

            if (informationRequest == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            ModelState.Clear();

            var irn = new InformationRequestNote(informationRequest, informationRequestNote.Notes, CurrentUser.Identity.Name);
            irn.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _informationRequestNoteRepository.EnsurePersistent(irn);

                Message = string.Format(Messages.Saved, "Note");
                return this.RedirectToAction(a => a.Edit(id));
            }
            

            return View(irn);
        }

    }

	
	/// <summary>
    /// ViewModel for the InformationRequest class
    /// </summary>
    public class InformationRequestViewModel
	{
		public InformationRequest InformationRequest { get; set; }
 
		public static InformationRequestViewModel Create(IRepository repository, InformationRequest informationRequest = null)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new InformationRequestViewModel {InformationRequest = informationRequest ?? new InformationRequest()};
 
			return viewModel;
		}
	}
}
