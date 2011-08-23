using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using Agribusiness.Import.Helpers;
using Agribusiness.Import.Models;
using Agribusiness.Import.Models.View;

namespace Agribusiness.Import.Controllers
{
    public class ContactFirmsController : ApplicationController
    {
        private static readonly string _rContactFirm = "RContactFirm";
        private static readonly string _archiveRContactFirm = "ArchiveRContactFirm";

        public ActionResult ContactFirms()
        {
            var viewModel = ReadContactFirm();
            return View(viewModel);
        }

        public ActionResult ArchiveContactFirms()
        {
            var viewModel = ReadArchiveContactFirm();
            return View(viewModel);
        }

        public static ContactFirmViewModel ReadContactFirm()
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
            return viewModel;
        }

        public static ContactFirmViewModel ReadArchiveContactFirm()
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
            return viewModel;
        }

        private static void ReadData(string file, bool imported, List<ContactFirms> contactFirms, List<KeyValuePair<string, string>> errors)
        {
            var sheet = ExcelHelpers.OpenWorkbook(HostingEnvironment.MapPath(file));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    //var rcfId = ExcelHelpers.ReadIntCell(row, 2);

                    var c_id = ExcelHelpers.ReadIntCell(row, 0);

                    if (Db.ContactFirms.Any(a => a.ContactId == c_id)) throw new Exception("Already exists");
                
                    // check and see if we came across it already in this file
                    var item = contactFirms.Where(a => a.ContactId == c_id).FirstOrDefault();
                    if (item != null)
                    {
                        errors.Add(new KeyValuePair<string, string>(c_id.ToString(), "duplicate value, replacing old"));

                        // get the index to remove the first one
                        contactFirms.Remove(item);
                    }

                    var cfirm = new ContactFirms();

                    cfirm.ContactId = ExcelHelpers.ReadIntCell(row, 0);
                    cfirm.FirmId = ExcelHelpers.ReadIntCell(row, 1);
                    cfirm.rcfId = ExcelHelpers.ReadIntCell(row, 2);
                    cfirm.Title = ExcelHelpers.ReadCell(row, 3);

                    contactFirms.Add(cfirm);

                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(row.GetCell(0).ToString(), ex.Message));
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
