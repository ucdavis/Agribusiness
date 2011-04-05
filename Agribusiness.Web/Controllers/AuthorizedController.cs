﻿using System;
using System.Web.Mvc;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Home controller for authorized users.
    /// </summary>
    public class AuthorizedController : ApplicationController
    {
        private readonly ISeminarService _seminarService;

        public AuthorizedController(ISeminarService seminarService)
        {
            _seminarService = seminarService;
        }

        public ActionResult Index()
        {
            var viewModel = AuthorizedViewModel.Create(Repository, _seminarService, CurrentUser.Identity.Name);

            return View(viewModel);
        }
    }
}
