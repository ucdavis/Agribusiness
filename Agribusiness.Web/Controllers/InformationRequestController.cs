using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the InformationRequest class
    /// </summary>
    public class InformationRequestController : ApplicationController
    {
	    private readonly IRepository<InformationRequest> _informationrequestRepository;

        public InformationRequestController(IRepository<InformationRequest> informationrequestRepository)
        {
            _informationrequestRepository = informationrequestRepository;
        }
    
        //
        // GET: /InformationRequest/
        public ActionResult Index()
        {
            var informationrequestList = _informationrequestRepository.Queryable.OrderBy(a=>a.Responded);

            return View(informationrequestList);
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
