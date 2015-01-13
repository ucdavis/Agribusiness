using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Firm class
    /// </summary>
    [UserOnly]
    public class FirmController : ApplicationController
    {
	    private readonly IRepository<Firm> _firmRepository;
        private readonly IFirmService _firmService;

        public FirmController(IRepository<Firm> firmRepository, IFirmService firmService)
        {
            _firmRepository = firmRepository;
            _firmService = firmService;
        }

        //
        // GET: /Firm/
        public ActionResult Index()
        {
            return View(FirmListViewModel.Create(_firmRepository, _firmService));
        }

        public ActionResult Edit(int id, int? decissionId = null, Guid? userId = null, int? seminarId = null)
        {
            var firm = _firmRepository.GetNullableById(id);

            if (firm == null)
            {
                Message = string.Format(Messages.NotFound, "firm", id);
                return this.RedirectToAction(a => a.Index());
            }

            // if review, get the last one, if it exists
            var origFirm = firm.Review ? (_firmService.GetFirm(firm.FirmCode)) : firm;

            var viewModel = FirmViewModel.Create(Repository, firm, origFirm);

            ViewBag.DecissionId = decissionId;
            ViewBag.UserId = userId;
            ViewBag.SeminarId = seminarId;

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, Firm firm, int? decissionId = null)
        {
            ViewBag.DecissionId = decissionId;
            firm.Review = false;

            if (ModelState.IsValid)
            {
                _firmRepository.EnsurePersistent(firm);
                Message = string.Format(Messages.Saved, "Firm");
                if(decissionId != null)
                {
                    return this.RedirectToAction<SeminarApplicationController>(a => a.Decide(decissionId.Value));
                }
                return this.RedirectToAction(a => a.Index());
            }
            
            // if review, get the last one, if it exists
            var origFirm = firm.Review ? (_firmService.GetFirm(firm.FirmCode)) : firm;

            var viewModel = FirmViewModel.Create(Repository, firm, origFirm);
            return View(viewModel);            
        }

        /// <summary>
        /// Reject requested changes on firms marked for review
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RedirectToRouteResult Reject(int id)
        {
            var firm = _firmRepository.GetNullableById(id);

            if (firm == null)
            {
                Message = string.Format(Messages.NotFound, "firm", id);
                return this.RedirectToAction(a => a.Index());
            }

            if (!firm.Review)
            {
                Message = string.Format("Firm {0}({1}) was not marked for review", firm.Name, id);
                return this.RedirectToAction(a => a.Index());
            }

            var firmName = firm.Name;

            // delete the firm
            _firmRepository.Remove(firm);
            Message = string.Format("Suggested changes to {0} have been discarded", firmName);
            return this.RedirectToAction(a => a.Index());
        }
    }
}
