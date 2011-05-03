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
    public class ContactFirmsController : ApplicationController
    {
        private readonly string _rContactFirm = "RContactFirm";
        private readonly string _archiveRContactFirm = "ArchiveRContactFirm";

        public ActionResult ContactFirms()
        {
            var imported = Db.Trackings.Where(a => a.Name == _rContactFirm).Any();

            var cfirms = new List<ContactFirms>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/R_Contact_Firm.xls", imported, cfirms, errors);


            if (!imported)
            {
                var tracking = new Tracking() { Name = _rContactFirm };
                Db.Trackings.Add(tracking);
                Db.SaveChanges();
            }

            var viewModel = ContactFirmViewModel.Create(cfirms, errors, imported);
            return View(viewModel);
        }

        public ActionResult ArchiveContactFirms()
        {
            var imported = Db.Trackings.Where(a => a.Name == _archiveRContactFirm).Any();

            var cfirms = new List<ContactFirms>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/archived_R_Contact_Firm.xls", imported, cfirms, errors);

            if (!imported)
            {
                var tracking = new Tracking() { Name = _archiveRContactFirm };
                Db.Trackings.Add(tracking);
            }

            var viewModel = ContactFirmViewModel.Create(cfirms, errors, imported);
            return View(viewModel);
        }

        private void ReadData(string file, bool imported, List<ContactFirms> contactFirms, List<KeyValuePair<string, string>> errors)
        {
            var sheet = ExcelHelpers.OpenWorkbook(Server.MapPath(file));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    var rcfId = ExcelHelpers.ReadIntCell(row, 2);
                    if (Db.ContactFirms.Any(a => a.rcfId == rcfId)) throw new Exception("Already exists");
                
                    // check and see if we came across it already in this file
                    var item = contactFirms.Where(a => a.rcfId == rcfId).FirstOrDefault();
                    if (item != null)
                    {
                        errors.Add(new KeyValuePair<string, string>(rcfId.ToString(), "duplicate value, replacing old"));

                        // get the index to remove the first one
                        contactFirms.Remove(item);
                    }

                    var cfirm = new ContactFirms();

                    cfirm.ContactId = ExcelHelpers.ReadIntCell(row, 0);
                    cfirm.FirmId = ExcelHelpers.ReadIntCell(row, 1);
                    cfirm.rcfId = ExcelHelpers.ReadIntCell(row, 2);

                    contactFirms.Add(cfirm);

                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(row.GetCell(13).ToString(), ex.Message));
                }

            }

            if (!imported)
            {
                foreach (var a in contactFirms)
                {
                    Db.ContactFirms.Add(a);    
                }
                

                Db.SaveChanges();
            }

            
        }
    }
}
