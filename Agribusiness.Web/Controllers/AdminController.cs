using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using AutoMapper;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Admin class
    /// </summary>
    [UserOnly]
    public class AdminController : ApplicationController
    {
        private readonly IRepository<Commodity> _commodityRepository;

        public AdminController(IRepository<Commodity> commodityRepository)
        {
            _commodityRepository = commodityRepository;
        }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
    }
}
