using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
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

        public ActionResult Edit(int id)
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
            return View(viewModel);
        }
    }
}
