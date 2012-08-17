using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Microsoft.Reporting.WebForms;

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

        public FileResult GetReport(Report report, int? seminarId)
        {
            throw new NotImplementedException();
        }

        public enum Report {}

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
