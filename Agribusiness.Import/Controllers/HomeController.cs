using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Import.Helpers;
using Agribusiness.Import.Models;
using Agribusiness.Import.Models.View;

namespace Agribusiness.Import.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportAll()
        {
            var results = new List<ImportResult>();

            var firm = FirmController.ReadFirms();
            results.Add(new ImportResult(){File="Firms", Error = firm.Errors.Count, Read = firm.Firms.Count, Imported = firm.AlreadyImported});
            firm = FirmController.ReadArchiveFirms();
            results.Add(new ImportResult() { File = "Archive Firms", Error = firm.Errors.Count, Read = firm.Firms.Count, Imported = firm.AlreadyImported });

            var cfirm = ContactFirmsController.ReadContactFirm();
            results.Add(new ImportResult() { File="Contact Firm", Error = cfirm.Errors.Count, Read = cfirm.ContactFirms.Count, Imported = cfirm.AlreadyImported });
            cfirm = ContactFirmsController.ReadArchiveContactFirm();
            results.Add(new ImportResult() { File = "Archive Contact Firm", Error = cfirm.Errors.Count, Read = cfirm.ContactFirms.Count, Imported = cfirm.AlreadyImported });

            var seminar = SeminarController.ReadSeminar();
            results.Add(new ImportResult() { File = "Seminar", Error = seminar.Errors.Count, Read = seminar.Seminars.Count, Imported = seminar.AlreadyImported });
            seminar = SeminarController.ReadArchiveSeminar();
            results.Add(new ImportResult() { File = "Archive Seminar", Error = seminar.Errors.Count, Read = seminar.Seminars.Count, Imported = seminar.AlreadyImported });

            var contact = ContactController.ReadContact();
            results.Add(new ImportResult() { File = "Contact", Error = contact.Errors.Count, Read = contact.Contacts.Count, Imported = contact.AlreadyImported });
            contact = ContactController.ReadArchiveContact();
            results.Add(new ImportResult() { File = "Archive Contact", Error = contact.Errors.Count, Read = contact.Contacts.Count, Imported = contact.AlreadyImported });

            var commodities = CommodityController.ReadCommodity();
            results.Add(new ImportResult() { File = "Commodity", Error = commodities.Errors.Count, Read = commodities.Commodities.Count, Imported = commodities.AlreadyImported });
            commodities = CommodityController.ReadArchiveCommodity();
            results.Add(new ImportResult() { File = "Archive Commodity", Error = commodities.Errors.Count, Read = commodities.Commodities.Count, Imported = commodities.AlreadyImported });

            return View(results);
        }
    }

    public class ImportResult
    {
        public string File { get; set; }
        public int Read { get; set; }
        public int Error { get; set; }
        public bool Imported { get; set; }
    }
}
