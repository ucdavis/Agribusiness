using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Import.Models;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Agribusiness.Import.Controllers
{
    public class FirmController : Controller
    {
        protected readonly AgribusinessContext Db = new AgribusinessContext();

        //
        // GET: /Firm/

        public ActionResult Index()
        {
            var firms = new List<Firm>();
            var errors = new List<KeyValuePair<string, string>>();

            var stream = new FileStream(Server.MapPath("~/Assets/Firm.xls"), FileMode.Open);
            var workbook = new HSSFWorkbook(stream);
            var sheet = workbook.GetSheetAt(0);

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    var test = ReadCell(row, 6);

                    var firm = new Firm();
                    firm.City = ReadCell(row, 0);           // a
                    firm.Country = ReadCell(row, 1);        // b
                    firm.State = ReadCell(row, 2);          // c
                    firm.Address = ReadCell(row, 3);        // d
                    firm.Zip = ReadCell(row, 4);            // e
                    firm.IsPOBox = ReadBoolCell(row, 5);    // f
                    firm.Fax = ReadCell(row, 6);            // g
                    firm.Phone = ReadCell(row, 7);          // h
                    firm.Ext = ReadCell(row, 8);            // i
                    firm.Created = ReadDateCell(row, 9);    // j
                    firm.Modified = ReadDateCell(row, 10);  // k
                    firm.Description = ReadCell(row, 11);   // l
                    firm.Financial = ReadBoolCell(row, 12); // m
                    firm.Id = ReadIntCell(row, 13);         // n
                    firm.Name = ReadCell(row, 14);          // o
                    firm.CreatedBy = ReadCell(row, 15);     // p
                    firm.ModifiedBy = ReadCell(row, 16);    // q
                    firm.WebAddress = ReadCell(row, 17);    // r

                    firms.Add(firm);

                    Db.Firms.Add(firm);
                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(row.GetCell(13).ToString(), ex.Message));
                }

            }

            Db.SaveChanges();

            var viewModel = FirmViewModel.Create(firms, errors);
            return View(viewModel);
        }

        private string ReadCell(Row row, int index)
        {
            var val = row.GetCell(index);

            return val != null ? val.ToString() : null;
        }

        private int? ReadIntCell(Row row, int index)
        {
            var val = row.GetCell(index);

            int num;
            if (val != null && int.TryParse(val.ToString(), out num))
            {
                return num;
            }

            return null;
        }

        private DateTime? ReadDateCell(Row row, int index)
        {
            var val = row.GetCell(index);

            DateTime date;
            if (val != null && DateTime.TryParse(val.ToString(), out date))
            {
                return date;
            }

            return null;
        }

        private bool? ReadBoolCell(Row row, int index)
        {
            var val = row.GetCell(index);

            if (val != null)
            {
                return val.ToString() == "yes" ? true : false;
            }

            return null;
        }
    }
   
    public class FirmViewModel
    {
        public IList<Firm> Firms { get; set; }
        public IList<KeyValuePair<string, string>> Errors { get; set; }

        public static FirmViewModel Create(IList<Firm> firms, IList<KeyValuePair<string, string>> errors)
        {
            var viewModel = new FirmViewModel() {Firms = firms, Errors = errors};
            return viewModel;
        }
    }
}
