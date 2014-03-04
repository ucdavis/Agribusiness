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
    public class CommodityController : ApplicationController
    {
        private static readonly string _commodityTable = "CommodityTable";
        private static readonly string _archiveCommodityTable = "ArchiveCommodityTable";

        public ActionResult Commodity()
        {
            var viewModel = ReadCommodity();
            return View(viewModel);
        }

        public ActionResult ArchiveCommodity()
        {
            var viewModel = ReadArchiveCommodity();
            return View(viewModel);
        }

        public static CommodityViewModel ReadCommodity()
        {
            var imported = Db.Trackings.Where(a => a.Name == _commodityTable).Any();

            var commodities = new List<CommodityLink>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/Commodity.xls", imported, commodities, errors);

            if (!imported)
            {
                var tracking = new Tracking() { Name = _commodityTable };
                Db.Trackings.Add(tracking);
                Db.SaveChanges();
            }

            var viewModel = CommodityViewModel.Create(commodities, errors, imported);
            return viewModel;
        }

        public static CommodityViewModel ReadArchiveCommodity()
        {
            var imported = Db.Trackings.Where(a => a.Name == _archiveCommodityTable).Any();

            var commodities = new List<CommodityLink>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/archived_Commodity.xls", imported, commodities, errors);

            if (!imported)
            {
                var tracking = new Tracking() { Name = _archiveCommodityTable };
                Db.Trackings.Add(tracking);
                Db.SaveChanges();
            }

            var viewModel = CommodityViewModel.Create(commodities, errors, imported);
            return viewModel;
        }

        public static void ReadData(string file, bool imported, List<CommodityLink> commodities, List<KeyValuePair<string, string>> errors)
        {
            var sheet = ExcelHelpers.OpenWorkbook(HostingEnvironment.MapPath(file));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    var m_id = ExcelHelpers.ReadIntCell(row, 4);

                    // check the existance
                    if (Db.CommodityLinks.Any(a => a.m_id == m_id)) throw new Exception("Already exists");

                    // check and see if we came across it already in this file
                    var item = commodities.Where(a => a.m_id == m_id).FirstOrDefault();
                    if (item != null)
                    {
                        errors.Add(new KeyValuePair<string, string>(m_id.ToString(), "duplicate value, replacing old"));

                        // get the index to remove the first one
                        commodities.Remove(item);
                    }

                    var commodity = new CommodityLink();

                    commodity.DateCreated =     ExcelHelpers.ReadDateCell(row, 0);      // a
                    commodity.DateModified =    ExcelHelpers.ReadDateCell(row, 1);      // b
                    commodity.FirmId =          ExcelHelpers.ReadIntCell(row, 2);       // c
                    commodity.RContactFirmId =  ExcelHelpers.ReadIntCell(row, 3);       // d
                    commodity.m_id =            ExcelHelpers.ReadIntCell(row, 4);       // e
                    commodity.Name =            ExcelHelpers.ReadCell(row, 5);          // f
                    commodity.CreatedBy =       ExcelHelpers.ReadCell(row, 6);          // g
                    commodity.ModifiedBy =      ExcelHelpers.ReadCell(row, 7);          // h

                    if (!string.IsNullOrWhiteSpace(commodity.Name))
                    {
                        foreach (var a in commodity.Name.Split(','))
                        {
                            commodity.AddCommodity(new Commodity(){Name=Capitalize(a.Trim())});
                        }
                    }

                    commodities.Add(commodity);
                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(ExcelHelpers.ReadIntCell(row, 4).ToString(), ex.Message));
                }

            }

            // save all the instances
            if (!imported)
            {
                foreach (var a in commodities)
                {
                    Db.CommodityLinks.Add(a);
                }
                
                Db.SaveChanges();
            }
        }

        private static string Capitalize(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return input.Substring(0, 1).ToUpper() + input.Substring(1).ToLower();
        }
    }
}
