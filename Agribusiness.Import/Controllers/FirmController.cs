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
    public class FirmController : ApplicationController
    {
        private static readonly string _firmTable = "firms";
        private static readonly string _archiveFirmTable = "archiveFirms";

        public ActionResult Firms()
        {
            var viewModel = ReadFirms();
            return View(viewModel);
        }

        public ActionResult ArchivedFirms()
        {
            var viewModel = ReadArchiveFirms();
            return View(viewModel);
        }

        public static FirmViewModel ReadFirms()
        {
            var imported = Db.Trackings.Where(a => a.Name == _firmTable).Any();

            var firms = new List<Firm>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/Firm.xls", imported, firms, errors);

            if (!imported)
            {
                var tracking = new Tracking() { Name = _firmTable };
                Db.Trackings.Add(tracking);
                Db.SaveChanges();
            }

            var viewModel = FirmViewModel.Create(firms, errors, imported);
            return viewModel;
        }

        public static FirmViewModel ReadArchiveFirms()
        {
            var imported = Db.Trackings.Where(a => a.Name == _archiveFirmTable).Any();

            var firms = new List<Firm>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/Firm.xls", imported, firms, errors);

            if (!imported)
            {
                var tracking = new Tracking() { Name = _archiveFirmTable };
                Db.Trackings.Add(tracking);
                Db.SaveChanges();
            }

            var viewModel = FirmViewModel.Create(firms, errors, imported);
            return viewModel;
        }

        private static void ReadData(string file, bool imported, List<Firm> firms, List<KeyValuePair<string, string>> errors)
        {

            var sheet = ExcelHelpers.OpenWorkbook(HostingEnvironment.MapPath(file));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    var f_id = ExcelHelpers.ReadIntCell(row, 13);

                    // check the existance
                    if (Db.Firms.Any(a => a.f_id == f_id)) throw new Exception("Already exists");

                    // check and see if we came across it already in this file
                    var item = firms.Where(a => a.f_id == f_id).FirstOrDefault();
                    if (item != null)
                    {
                        errors.Add(new KeyValuePair<string, string>(f_id.ToString(), "duplicate value, replacing old"));

                        // get the index to remove the first one
                        firms.Remove(item);
                    }

                    var firm = new Firm();
                    firm.City = ExcelHelpers.ReadCell(row, 0);           // a
                    firm.Country = ExcelHelpers.ReadCell(row, 1);        // b
                    firm.State = ExcelHelpers.ReadCell(row, 2);          // c
                    firm.Address = ExcelHelpers.ReadCell(row, 3);        // d
                    firm.Zip = ExcelHelpers.ReadCell(row, 4);            // e
                    firm.IsPOBox = ExcelHelpers.ReadBoolCell(row, 5);    // f
                    firm.Fax = ExcelHelpers.ReadCell(row, 6);            // g
                    firm.Phone = ExcelHelpers.ReadCell(row, 7);          // h
                    firm.Ext = ExcelHelpers.ReadCell(row, 8);            // i
                    firm.Created = ExcelHelpers.ReadDateCell(row, 9);    // j
                    firm.Modified = ExcelHelpers.ReadDateCell(row, 10);  // k
                    firm.Description = ExcelHelpers.ReadCell(row, 11);   // l
                    firm.Financial = ExcelHelpers.ReadBoolCell(row, 12); // m
                    firm.f_id = ExcelHelpers.ReadIntCell(row, 13);         // n
                    firm.Name = ExcelHelpers.ReadCell(row, 14);          // o
                    firm.CreatedBy = ExcelHelpers.ReadCell(row, 15);     // p
                    firm.ModifiedBy = ExcelHelpers.ReadCell(row, 16);    // q
                    firm.WebAddress = ExcelHelpers.ReadCell(row, 17);    // r

                    // check the name against one that has already been created
                    var db = Db.Firms.Where(a => a.Name == firm.Name).FirstOrDefault();
                    var list = firms.Where(a => a.Name == firm.Name).FirstOrDefault();
                    if (db != null)
                    {
                        firm.GroupId = db.GroupId;
                    }
                    else if (list != null)
                    {
                        firm.GroupId = db.GroupId;
                    }
                    else
                    {
                        firm.GroupId = Guid.NewGuid();
                    }

                    firms.Add(firm);

                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(ExcelHelpers.ReadIntCell(row, 13).ToString(), ex.Message));
                }

            }


            if (!imported)
            {
                foreach (var a in firms)
                {
                    Db.Firms.Add(a);    
                }
                

                Db.SaveChanges();
            }


            
        }
    }
}
