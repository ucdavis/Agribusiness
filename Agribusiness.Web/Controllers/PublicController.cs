﻿using System;
using System.Web.Mvc;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Public class
    /// </summary>
    public class PublicController : ApplicationController
    {
        public ActionResult Background()
        {
            return View();
        }

        public ActionResult SteeringCommittee()
        {
            return View();
        }

        public ActionResult ProgramOverview()
        {
            return View();
        }

        public ActionResult CaseExamples()
        {
            return View();
        }

        public ActionResult Venue()
        {
            return View();
        }

        public ActionResult McCorkleFund()
        {
            return View();
        }
    }

}
