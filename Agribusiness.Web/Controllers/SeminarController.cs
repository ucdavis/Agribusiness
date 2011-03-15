using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Seminar class
    /// </summary>
    public class SeminarController : ApplicationController
    {
	    private readonly IRepository<Seminar> _seminarRepository;

        public SeminarController(IRepository<Seminar> seminarRepository)
        {
            _seminarRepository = seminarRepository;
        }
    
        //
        // GET: /Seminar/
        [UserOnly]
        public ActionResult Index()
        {
            var seminarList = _seminarRepository.Queryable;

            return View(seminarList);
        }

        [UserOnly]
        public ActionResult Create()
        {
            var viewModel = SeminarViewModel.Create(Repository);

            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Create(Seminar seminar)
        {
            var viewModel = SeminarViewModel.Create(Repository, seminar);

            return View(viewModel);
        }

    }

	/// <summary>
    /// ViewModel for the Seminar class
    /// </summary>
    public class SeminarViewModel
	{
		public Seminar Seminar { get; set; }
 
		public static SeminarViewModel Create(IRepository repository, Seminar seminar = null)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new SeminarViewModel {Seminar = seminar ?? new Seminar()};
 
			return viewModel;
		}
	}
}
