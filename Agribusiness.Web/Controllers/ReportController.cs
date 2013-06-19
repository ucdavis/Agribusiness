using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Microsoft.Reporting.WebForms;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Report class
    /// </summary>
    [UserOnly]
    public class ReportController : ApplicationController
    {
        private readonly string _serverLocation = ConfigurationManager.AppSettings["ReportServer"];

        //
        // GET: /Report/
        public ActionResult Index()
        {
            var viewModel = ReportViewModel.Create(Repository, Site);
            return View(viewModel);
        }

        public FileResult GetReport(Report report, int? seminarId, string siteName)
        {
            var name = string.Empty;
            var parameters = new Dictionary<string, string>();
            var fileName = string.Empty;

            switch (report)
            {
                case Report.AllContactsBoth:
                    name = "AllContactsBoth";
                    fileName = name;
                    break;
                case Report.AllContactsBySite:
                    name = "AllContactsBySite";
                    parameters.Add("sitename", siteName);
                    fileName = string.Format("{0}{1}", name, siteName);
                    break;
                case Report.AllApplicantsBySite:
                    Check.Require(seminarId != null);
                    var seminar = RepositoryFactory.SeminarRepository.Queryable.Single(a => a.Id == seminarId.Value);
                    name = "AllApplicantsBySite";
                    parameters.Add("seminarId", seminarId.Value.ToString());
                    parameters.Add("SiteName", string.Format("{0} {1}", seminar.Year, seminar.Site.Id));
                    fileName = string.Format("{0}{1}", "AllApplicantsBySeminar", seminar.Site.Id);
                    break;

                case Report.AllAttendeesBySeminar:
                    Check.Require(seminarId != null);
                    seminar = RepositoryFactory.SeminarRepository.Queryable.Single(a => a.Id == seminarId.Value);
                    name = "AllAttendeesBySeminar";
                    parameters.Add("seminarId", seminarId.Value.ToString());
                    parameters.Add("SeminarName", string.Format("{0} {1}", seminar.Year, seminar.Site.Id));
                    fileName = string.Format("{0}{1}", "AllAttendeesBySeminar", seminar.Site.Id);
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }

            return File(GetReport(string.Format("/agribusiness/{0}", name), parameters), "application/excel", string.Format("{0}{1}.xls",DateTime.Now.Date.ToString("yyyyMMdd"), fileName));

        }

        public enum Report { AllApplicantsBySite, AllAttendeesBySeminar, AllContactsBoth, AllContactsBySite }

        private byte[] GetReport(string ReportName, Dictionary<string, string> parameters)
        {
            string reportServer = _serverLocation;

            var rview = new ReportViewer();
            rview.ServerReport.ReportServerUrl = new Uri(reportServer);
            rview.ServerReport.ReportPath = ReportName;

            var paramList = new List<ReportParameter>();

            if (parameters.Count > 0)
            {
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    paramList.Add(new ReportParameter(kvp.Key, kvp.Value));
                }
            }

            rview.ServerReport.SetParameters(paramList);

            string mimeType, encoding, extension, deviceInfo;
            string[] streamids;
            Warning[] warnings;

            string format = "Excel";

            deviceInfo =
            "<DeviceInfo>" +
            "<SimplePageHeaders>True</SimplePageHeaders>" +
            "<HumanReadablePDF>True</HumanReadablePDF>" +   // this line disables the compression done by SSRS 2008 so that it can be merged.
            "</DeviceInfo>";

            byte[] bytes = rview.ServerReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);

            return bytes;
        }
    }
}
