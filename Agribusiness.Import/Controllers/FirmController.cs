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
    public class FirmController : ApplicationController
    {
        private readonly string _firmTable = "firms";
        private readonly string _archiveFirmTable = "archiveFirms";

        public ActionResult Firms()
        {
            var imported = Db.Trackings.Where(a => a.Name == _firmTable).Any();

            var firms = new List<Firm>();
            var errors = new List<KeyValuePair<string, string>>();

            var sheet = ExcelHelpers.OpenWorkbook(Server.MapPath("~/Assets/Firm.xls"));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
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

                    firms.Add(firm);

                    if (!imported)
                    {
                        Db.Firms.Add(firm);
                    }

                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(row.GetCell(13).ToString(), ex.Message));
                }

            }

            if (!imported)
            {
                var tracking = new Tracking() { Name = _firmTable };
                Db.Trackings.Add(tracking);
            }

            Db.SaveChanges();

            var viewModel = FirmViewModel.Create(firms, errors, imported);
            return View(viewModel);
        }

        public ActionResult ArchivedFirms()
        {
            var imported = Db.Trackings.Where(a => a.Name == _archiveFirmTable).Any();

            var firms = new List<Firm>();
            var errors = new List<KeyValuePair<string, string>>();

            var sheet = ExcelHelpers.OpenWorkbook(Server.MapPath("~/Assets/Firm.xls"));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    var f_id = ExcelHelpers.ReadIntCell(row, 13);

                    // check the existance
                    if (Db.Firms.Any(a => a.f_id == f_id)) throw new Exception("Already exists");

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

                    firms.Add(firm);

                    if (!imported)
                    {
                        Db.Firms.Add(firm);
                    }

                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(ExcelHelpers.ReadIntCell(row, 13).ToString(), ex.Message));
                }

            }

            if (!imported)
            {
                var tracking = new Tracking() { Name = _archiveFirmTable };
                Db.Trackings.Add(tracking);
            }

            Db.SaveChanges();

            var viewModel = FirmViewModel.Create(firms, errors, imported);
            return View(viewModel);
        }
    }
}
