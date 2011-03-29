using System;
using System.IO;
using System.Web;
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
        private readonly IRepository<Seminar> _seminarRepository;

        public CaseStudyController(IRepository<CaseStudy> casestudyRepository, IRepository<Seminar> seminarRepository)
        {
            _casestudyRepository = casestudyRepository;
            _seminarRepository = seminarRepository;
        }

        /// <summary>
        /// Add a case study to a seminar
        /// </summary>
        /// <param name="seminarId"></param>
        /// <returns></returns>
        public ActionResult Create(int seminarId)
        {
            var seminar = _seminarRepository.GetNullableById(seminarId);

            if (seminar == null)
            {
                Message = string.Format(Messages.NotFound, "Seminar", seminarId);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var viewModel = CaseStudyViewModel.Create(Repository, seminar);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(int seminarId, CaseStudy caseStudy, HttpPostedFileBase file)
        {
            var seminar = Repository.OfType<Seminar>().GetNullableById(seminarId);

            if (file != null)
            {
                var reader = new BinaryReader(file.InputStream);
                var data = reader.ReadBytes(file.ContentLength);
                caseStudy.File = data;
            }

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

            var viewModel = CaseStudyViewModel.Create(Repository, seminar, caseStudy);
            return View(viewModel);
        }

        public ActionResult Edit(int id, int seminarId)
        {
            var seminar = _seminarRepository.GetNullableById(seminarId);
            var caseStudy = _casestudyRepository.GetNullableById(id);

            if (seminar == null || caseStudy == null)
            {
                Message += seminar == null ? string.Format(Messages.NotFound, "Seminar", seminarId) : string.Empty;
                Message += caseStudy == null ? string.Format(Messages.NotFound, "Case Study", id) : string.Empty;
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var viewModel = CaseStudyViewModel.Create(Repository, seminar, caseStudy);
            return View(viewModel);
        }

        /// <summary>
        /// Either adding a Author or Case Executive
        /// </summary>
        /// <returns></returns>
        public ActionResult AddPerson(int id)
        {
            var caseStudy = _casestudyRepository.GetNullableById(id);

            if (caseStudy == null)
            {
                Message = string.Format(Messages.NotFound, "case study", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var viewModel = CaseStudyPersonViewModel.Create(Repository, caseStudy);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AddPerson(int id, int seminarPersonId)
        {
            return View();
        }

        public FileResult Download(int id)
        {
            var caseStudy = _casestudyRepository.GetNullableById(id);

            if (caseStudy == null) return File(new byte[0], string.Empty);

            var fileName = caseStudy.Name.Replace(" ", string.Empty);

            return File(caseStudy.File, "application/pdf", string.Format("{0}.pdf", fileName));
        }
    }
}
