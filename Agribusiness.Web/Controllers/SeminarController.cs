using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

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

        #region Administrative Functions
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
            if (ModelState.IsValid)
            {
                _seminarRepository.EnsurePersistent(seminar);
                Message = string.Format(Messages.Saved, "Seminar");
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = SeminarViewModel.Create(Repository, seminar);
            return View(viewModel);
        }

        [UserOnly]
        public ActionResult Edit(int id)
        {
            var seminar = _seminarRepository.GetNullableById(id);

            if (seminar == null)
            {
                ErrorMessages = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = SeminarViewModel.Create(Repository, seminar);
            return View(viewModel);
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Edit(int id, Seminar seminar)
        {
            var origSeminar = _seminarRepository.GetNullableById(id);

            if (origSeminar == null)
            {
                ErrorMessages = string.Format(Messages.NotFound, "Seminar", id);
                return this.RedirectToAction(a => a.Index());
            }

            Mapper.Map(seminar, origSeminar);

            if (ModelState.IsValid)
            {
                _seminarRepository.EnsurePersistent(origSeminar);
                Message = string.Format(Messages.Saved, "Seminar");
                return this.RedirectToAction(a => a.Index());
            }

            var viewModel = SeminarViewModel.Create(Repository, seminar);
            return View(viewModel);
        }
        #endregion
    }
}
