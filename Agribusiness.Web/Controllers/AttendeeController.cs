using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Attendee class
    /// </summary>
    public class AttendeeController : ApplicationController
    {
        private readonly IRepository<Seminar> _seminarRespository;
        private readonly IPersonService _personService;

        public AttendeeController(IRepository<Seminar> seminarRespository, IPersonService personService)
        {
            _seminarRespository = seminarRespository;
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

    }
}
