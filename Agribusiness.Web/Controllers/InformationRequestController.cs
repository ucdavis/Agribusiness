using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
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
    [UserOnly]
    public class InformationRequestController : ApplicationController
    {
	    private readonly IRepository<InformationRequest> _informationrequestRepository;
        private readonly IRepository<InformationRequestNote> _informationRequestNoteRepository;
        private readonly IRepositoryWithTypedId<AddressType, char> _addressTypeRepository;
        private readonly IFirmService _firmService;

        public InformationRequestController(IRepository<InformationRequest> informationrequestRepository, IRepository<InformationRequestNote> informationRequestNoteRepository, IRepositoryWithTypedId<AddressType, char> addressTypeRepository, IFirmService firmService)
        {
            _informationrequestRepository = informationrequestRepository;
            _informationRequestNoteRepository = informationRequestNoteRepository;
            _addressTypeRepository = addressTypeRepository;
            _firmService = firmService;
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

        /// <summary>
        /// Create an account from Information Request
        /// </summary>
        /// <param name="id">Information Request Id</param>
        /// <returns></returns>
        public ActionResult CreatePerson(int id)
        {
            var ir = _informationrequestRepository.GetNullableById(id);

            if (ir == null)
            {
                Message = string.Format(Messages.NotFound, "information request", id);
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = PersonViewModel.Create(Repository, _firmService);

            // set the person's information
            var firstname = ir.Name.Trim().Substring(0, ir.Name.LastIndexOf(' '));
            var lastname = ir.Name.Trim().Substring(ir.Name.LastIndexOf(' '));

            viewModel.Person.FirstName = firstname.Trim();
            viewModel.Person.LastName = lastname.Trim();
            viewModel.Email = ir.Email;
            viewModel.UserName = string.Format("{0}.{1}", firstname.Trim(), lastname.Trim());

            // fake phone number
            viewModel.Person.Phone = "555-555-5555";    

            // fake address since we don't have it yet
            var atype = _addressTypeRepository.GetNullableById((char)StaticIndexes.Address_Business[0]);

            var address = viewModel.Addresses.Where(a => a.AddressType == atype).FirstOrDefault();
            address.Line1 = "Address";
            address.Zip = "Zip Code";
            
            // see if we can extract city/state out of the information reuqest
            var commaIndex = ir.Location.IndexOf(',');

            // no comma, probably no state information
            if (commaIndex < 0)
            {
                address.City = ir.Location;
            }
            // comma exists, most likely a state exists
            else
            {
                address.City = ir.Location.Substring(0, commaIndex).Trim();
                address.State = ir.Location.Substring(commaIndex + 1).Trim();
            }
            
            return View(viewModel);
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
