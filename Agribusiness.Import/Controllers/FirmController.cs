using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Import.Models;
using NPOI.HSSF.UserModel;

namespace Agribusiness.Import.Controllers
{
    public class FirmController : Controller
    {
        //
        // GET: /Firm/

        public ActionResult Index()
        {
            var firms = new List<Firm>();
            var errors = new List<KeyValuePair<string, string>>();
            
            using (var stream = new FileStream(Server.MapPath("~/Assets/Firm.xls"), FileMode.Open))
            {
                var workbook = new HSSFWorkbook(stream);
                var sheet = workbook.GetSheetAt(0);

                for (var i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);

                    try
                    {
                        var firm = new Firm();
                        firm.City = row.GetCell(0).ToString();      // a
                        firm.Country = row.GetCell(1).ToString();   // b
                        firm.State = row.GetCell(2).ToString();     // c
                        firm.Address = row.GetCell(3).ToString();   // d
                        firm.Zip = row.GetCell(4).ToString();       // e
                        firm.IsPOBox = row.GetCell(5).ToString() == "yes" ? true : false;   // f
                        firm.Fax = row.GetCell(6).ToString();       // g
                        firm.Phone = row.GetCell(7).ToString();     // h
                        firm.Ext = row.GetCell(8).ToString();       // i
                        firm.Created = DateTime.Parse(row.GetCell(9).ToString());   // j
                        firm.Modified = DateTime.Parse(row.GetCell(10).ToString()); // k
                        firm.Description = row.GetCell(11).ToString();   // L
                        firm.Financial = row.GetCell(12).ToString() == "yes" ? true : false;    // M
                        firm.Id = Convert.ToInt32(row.GetCell(13)); // N
                        firm.Name = row.GetCell(14).ToString();     // O
                        firm.CreatedBy = row.GetCell(15).ToString();    // P
                        firm.ModifiedBy = row.GetCell(16).ToString();   // Q
                        firm.WebAddress = row.GetCell(17).ToString();   // R

                        firms.Add(firm);
                    }
                    catch (Exception ex)
                    {
                        errors.Add(new KeyValuePair<string, string>(row.GetCell(13).ToString(), ex.Message));
                    }

                }
            }

            var viewModel = FirmViewModel.Create(firms, errors);
            return View(viewModel);
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
