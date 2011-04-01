﻿using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
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
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;

        public CaseStudyController(IRepository<CaseStudy> casestudyRepository, IRepository<Seminar> seminarRepository, IRepository<SeminarPerson> seminarPersonRepository)
        {
            _casestudyRepository = casestudyRepository;
            _seminarRepository = seminarRepository;
            _seminarPersonRepository = seminarPersonRepository;
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
        public ActionResult AddPerson(int id, int seminarPersonId, string personType)
        {
            var caseStudy = _casestudyRepository.GetNullableById(id);

            if (caseStudy == null)
            {
                Message = string.Format(Messages.NotFound, "case study", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var selectedPerson = _seminarPersonRepository.GetNullableById(seminarPersonId);

            if (selectedPerson == null)
            {
                ModelState.AddModelError("Seminar Person", "Invalid seminar person was selected.");
            }

            // check to make sure the role is valid
            if (personType != StaticIndexes.Role_CaseStudyAuthor && personType != StaticIndexes.Role_CaseExecutive)
            {
                ModelState.AddModelError("Person Type", "Invalid person type was selected.");
            }

            // check to make sure the person doesn't already exist in the list
            if (personType == StaticIndexes.Role_CaseStudyAuthor && caseStudy.CaseAuthors.Any(a => a == selectedPerson))
            {
                ModelState.AddModelError("Seminar Person", "Seminar person already exists as a case author for this case study.");
            }
            if (personType == StaticIndexes.Role_CaseExecutive && caseStudy.CaseExecutives.Any(a => a == selectedPerson))
            {
                ModelState.AddModelError("Seminar Person", "Seminar person already exists as a case executive for this case study.");
            }

            if (ModelState.IsValid)
            {
                if (personType == StaticIndexes.Role_CaseStudyAuthor)
                {
                    caseStudy.CaseAuthors.Add(selectedPerson);
                }
                if (personType == StaticIndexes.Role_CaseExecutive)
                {
                    caseStudy.CaseExecutives.Add(selectedPerson);
                }

                _casestudyRepository.EnsurePersistent(caseStudy);
                Message = string.Format(Messages.Saved, "Case Study");
                return this.RedirectToAction(a => a.Edit(id, caseStudy.Seminar.Id));
            }

            var viewModel = CaseStudyPersonViewModel.Create(Repository, caseStudy);
            viewModel.SeminarPersonId = seminarPersonId;
            return View(viewModel);
        }

        [HttpPost]
        public RedirectToRouteResult RemovePerson(int id, int seminarPersonId, string personType)
        {
            var caseStudy = _casestudyRepository.GetNullableById(id);

            if (caseStudy == null)
            {
                Message = string.Format(Messages.NotFound, "case study", id);
                return this.RedirectToAction<SeminarController>(a => a.Index());
            }

            var selectedPerson = _seminarPersonRepository.GetNullableById(seminarPersonId);

            if (selectedPerson == null)
            {
                Message = string.Format(Messages.NotFound, "seminar person", seminarPersonId);
                return this.RedirectToAction(a => a.Edit(id, caseStudy.Seminar.Id));
            }

            if (personType == StaticIndexes.Role_CaseStudyAuthor)
            {
                caseStudy.CaseAuthors.Remove(selectedPerson);
                
            }
            else if (personType == StaticIndexes.Role_CaseExecutive)
            {
                caseStudy.CaseExecutives.Remove(selectedPerson);
            }
            // invalid seminar role isn't valid for what we are doing
            else
            {
                Message = "An invalid role was provided.";
                return this.RedirectToAction(a => a.Edit(id, caseStudy.Seminar.Id));
            }

            _casestudyRepository.EnsurePersistent(caseStudy);

            Message = string.Format(Messages.Saved, "Case Study");
            return this.RedirectToAction(a => a.Edit(id, caseStudy.Seminar.Id));
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