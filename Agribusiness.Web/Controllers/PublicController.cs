﻿using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// All public facing static pages.
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

        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult MoreInformation()
        {
            return View();
        }

        [CaptchaValidator]
        [HttpPost]
        public ActionResult MoreInformation(InformationRequest informationRequest)
        {
            return View();
        }
    }

}
