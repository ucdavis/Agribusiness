using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Attendee class
    /// </summary>
    public class AttendeeController : ApplicationController
    {
        private readonly IRepository<SeminarPerson> _seminarPersonRespository;

        public AttendeeController(IRepository<SeminarPerson> seminarPersonRespository)
        {
            _seminarPersonRespository = seminarPersonRespository;
        }

        //
        // GET: /Attendee/
        public ActionResult Index()
        {
            var attendeeList = _seminarPersonRespository.Queryable;

            return View(attendeeList);
        }

    }
	
	/// <summary>
    /// ViewModel for the Attendee class
    /// </summary>
    public class AttendeeViewModel
	{
		public SeminarPerson SeminarPerson { get; set; }
 
		public static AttendeeViewModel Create(IRepository repository, SeminarPerson seminarPerson = null)
		{
			Check.Require(repository != null, "Repository must be supplied");
			
			var viewModel = new AttendeeViewModel {SeminarPerson = seminarPerson ?? new SeminarPerson()};
 
			return viewModel;
		}
	}
}
