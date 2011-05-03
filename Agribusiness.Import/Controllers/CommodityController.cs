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
    public class CommodityController : ApplicationController
    {
        private readonly string _commodityTable = "CommodityTable";
        private readonly string _archiveCommodityTable = "ArchiveCommodityTable";

        public ActionResult Commodity()
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
            return View(viewModel);
        }

        public ActionResult ArchiveCommodity()
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
            return View(viewModel);
        }

        public void ReadData(string file, bool imported, List<CommodityLink> commodities, List<KeyValuePair<string, string>> errors)
        {
            var sheet = ExcelHelpers.OpenWorkbook(Server.MapPath(file));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    var m_id = ExcelHelpers.ReadIntCell(row, 4);

                    // check the existance
                    if (Db.CommodityLinks.Any(a => a.m_id == m_id)) throw new Exception("Already exists");

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
                            commodity.AddCommodity(new Commodity(){Name=a.Trim()});
                        }
                    }

                    commodities.Add(commodity);

                    if (!imported)
                    {
                        Db.CommodityLinks.Add(commodity);
                    }

                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(ExcelHelpers.ReadIntCell(row, 4).ToString(), ex.Message));
                }

            }

            Db.SaveChanges();
        }
    }
}
