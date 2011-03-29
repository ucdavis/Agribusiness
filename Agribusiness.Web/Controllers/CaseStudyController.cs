using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the CaseStudy class
    /// </summary>
    public class CaseStudyController : ApplicationController
    {
	    private readonly IRepository<CaseStudy> _casestudyRepository;

        public CaseStudyController(IRepository<CaseStudy> casestudyRepository)
        {
            _casestudyRepository = casestudyRepository;
        }

        /// <summary>
        /// Add a case study to a seminar
        /// </summary>
        /// <param name="seminarId"></param>
        /// <returns></returns>
        public ActionResult Create(int seminarId)
        {
            var seminar = Repository.OfType<Seminar>().GetNullableById(seminarId);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", seminarId);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var viewModel = CaseStudyViewModel.Create(Repository, seminar);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(int seminarId, CaseStudy caseStudy)
        {
            var seminar = Repository.OfType<Seminar>().GetNullableById(seminarId);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", seminarId);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            caseStudy.Seminar = seminar;

            ModelState.Clear();
            caseStudy.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _casestudyRepository.EnsurePersistent(caseStudy);
                Message = string.Format(Messages.Saved, "Case Study");
                return this.RedirectToAction<SeminarController>(a => a.Edit(seminarId));
            }

            var viewModel = CaseStudyViewModel.Create(Repository, seminar);
            return View(viewModel);
        }
    }
}
