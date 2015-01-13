using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Services;
using Ionic.Zip;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using UCDArch.Core.PersistanceSupport;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Export class
    /// </summary>
    [UserOnly]
    public class ExportController : ApplicationController
    {
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;

        public ExportController(IRepository<SeminarPerson> seminarPersonRepository)
        {
            _seminarPersonRepository = seminarPersonRepository;
        }

        //
        // GET: /Export/
        public FileResult Index()
        {
            var seminar = SiteService.GetLatestSeminar(Site);

            var people = _seminarPersonRepository.Queryable.Where(a => a.Seminar == seminar).ToList();

            var stream = new MemoryStream();

            // create the zip file
            using (var zip = new ZipFile())
            {
                foreach (var person in people)
                {
                    if (person.Person != null) // && person.Person.OriginalPicture != null)
                    {
                        if (person.Person.OriginalPicture != null)
                        {
                            zip.AddEntry(
                                string.Format("{0}.{1}.{2}", person.Person.LastName.Trim(),
                                              person.Person.FirstName.Trim(),
                                              ExtractExtension(person.Person.ContentType)),
                                person.Person.OriginalPicture);
                        }

                        if (person.Person.MainProfilePicture != null)
                        {
                            zip.AddEntry(
                                string.Format("{0}.{1}-profile.{2}", person.Person.LastName.Trim(),
                                              person.Person.FirstName.Trim(),
                                              ExtractExtension(person.Person.ContentType)),
                                person.Person.MainProfilePicture);
                        }

                        if (person.Person.ThumbnailPicture != null)
                        {
                            zip.AddEntry(
                                string.Format("{0}.{1}-thumbnail.{2}", person.Person.LastName.Trim(),
                                              person.Person.FirstName.Trim(),
                                              ExtractExtension(person.Person.ContentType)),
                                person.Person.ThumbnailPicture);
                        }
                    }
                }

                zip.Save(stream);

                return File(stream.ToArray(), "application/zip", "agribusiness-export.zip");
            }

            return File(new byte[0], string.Empty);
        }

        private string ExtractExtension(string contentType)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    return contentType.Substring(contentType.IndexOf('/') + 1);
                }
            }
            catch
            {
            }

            // default to jpg
            return "jpg";
        }

        public FileResult AttendeeList()
        {
            var seminar = SiteService.GetLatestSeminar(Site);

            // get all the peeps
            var people = _seminarPersonRepository.Queryable.Where(a => a.Seminar == seminar).ToList();

            try
            {
                // Opening the Excel template...
                var fs = new FileStream(Server.MapPath(@"~\Content\NPOITemplate.xls"), FileMode.Open, FileAccess.Read);

                // Getting the complete workbook...
                var templateWorkbook = new HSSFWorkbook(fs, true);

                // Getting the worksheet by its name...
                var sheet = templateWorkbook.GetSheetAt(0); // GetSheet("Sheet1");

                // Getting the row... 0 is the first row. aka title row
                var title1 = sheet.CreateRow(0);

                var bold = templateWorkbook.CreateFont();

                var tcell1 = title1.CreateCell(10);
                tcell1.SetCellValue("Business Address");
                tcell1.CellStyle = templateWorkbook.CreateCellStyle();
                tcell1.CellStyle.Alignment = HorizontalAlignment.CENTER;
                tcell1.CellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_BLUE.index;

                var tcell2 = title1.CreateCell(16);
                tcell2.SetCellValue("Courier Address");
                tcell2.CellStyle = templateWorkbook.CreateCellStyle();
                tcell2.CellStyle.Alignment = HorizontalAlignment.CENTER;
                tcell2.CellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.LIGHT_GREEN.index;

                // merge the cells
                var cra = new CellRangeAddress(0, 0, 10, 15);
                sheet.AddMergedRegion(cra);
                var cra2 = new CellRangeAddress(0, 0, 16, 21);
                sheet.AddMergedRegion(cra2);

                var title2 = sheet.CreateRow(1);
                title2.CreateCell(0).SetCellValue("Salutation");
                title2.CreateCell(1).SetCellValue("First Name");
                title2.CreateCell(2).SetCellValue("MI");
                title2.CreateCell(3).SetCellValue("Last Name");
                title2.CreateCell(4).SetCellValue("Phone");
                title2.CreateCell(5).SetCellValue("Ext.");
                title2.CreateCell(6).SetCellValue("Title");
                title2.CreateCell(7).SetCellValue("Firm");
                title2.CreateCell(8).SetCellValue("FirmType");
                title2.CreateCell(9).SetCellValue("Commodities");

                // business
                title2.CreateCell(10).SetCellValue("Line 1");
                title2.CreateCell(11).SetCellValue("Line 2");
                title2.CreateCell(12).SetCellValue("City");
                title2.CreateCell(13).SetCellValue("State");
                title2.CreateCell(14).SetCellValue("Zip");
                title2.CreateCell(15).SetCellValue("Contry");

                // courier
                title2.CreateCell(16).SetCellValue("Line 1");
                title2.CreateCell(17).SetCellValue("Line 2");
                title2.CreateCell(18).SetCellValue("City");
                title2.CreateCell(19).SetCellValue("State");
                title2.CreateCell(20).SetCellValue("Zip");
                title2.CreateCell(21).SetCellValue("Contry");

                title2.CreateCell(22).SetCellValue("Res #");
                title2.CreateCell(23).SetCellValue("Check-In");
                title2.CreateCell(24).SetCellValue("Check-Out");

                for (var i = 0; i < people.Count; i++)
                {
                    // seminar person object
                    var person = people[i];
                    var dataRow = sheet.CreateRow(i + 2);

                    // fill the data
                    dataRow.CreateCell(0).SetCellValue(person.Person.Salutation);
                    dataRow.CreateCell(1).SetCellValue(person.Person.FirstName);
                    dataRow.CreateCell(2).SetCellValue(person.Person.MI);
                    dataRow.CreateCell(3).SetCellValue(person.Person.LastName);
                    dataRow.CreateCell(4).SetCellValue(person.Person.Phone);
                    dataRow.CreateCell(5).SetCellValue(person.Person.PhoneExt);
                    dataRow.CreateCell(6).SetCellValue(person.Title);
                    dataRow.CreateCell(7).SetCellValue(person.Firm.Name);
                    dataRow.CreateCell(8).SetCellValue(person.FirmName);
                    dataRow.CreateCell(9).SetCellValue(person.GetCommodityList());

                    try
                    {
                        var business = person.Person.Addresses.Where(a => a.AddressType.Id == 'B').FirstOrDefault();
                        var courier = person.Person.Addresses.Where(a => a.AddressType.Id == 'C').FirstOrDefault();

                        if (business != null)
                        {
                            dataRow.CreateCell(10).SetCellValue(business.Line1);
                            dataRow.CreateCell(11).SetCellValue(business.Line2);
                            dataRow.CreateCell(12).SetCellValue(business.City);
                            dataRow.CreateCell(13).SetCellValue(business.State);
                            dataRow.CreateCell(14).SetCellValue(business.Zip);
                            dataRow.CreateCell(15).SetCellValue(business.Country != null
                                                                    ? business.Country.Name
                                                                    : string.Empty);
                        }
                        else
                        {
                            dataRow.CreateCell(10).SetCellValue(string.Empty);
                            dataRow.CreateCell(11).SetCellValue(string.Empty);
                            dataRow.CreateCell(12).SetCellValue(string.Empty);
                            dataRow.CreateCell(13).SetCellValue(string.Empty);
                            dataRow.CreateCell(14).SetCellValue(string.Empty);
                            dataRow.CreateCell(15).SetCellValue(string.Empty);
                        }

                        if (courier != null)
                        {
                            dataRow.CreateCell(16).SetCellValue(courier.Line1);
                            dataRow.CreateCell(17).SetCellValue(courier.Line2);
                            dataRow.CreateCell(18).SetCellValue(courier.City);
                            dataRow.CreateCell(19).SetCellValue(courier.State);
                            dataRow.CreateCell(20).SetCellValue(courier.Zip);
                            dataRow.CreateCell(21).SetCellValue(courier.Country != null
                                                                    ? courier.Country.Name
                                                                    : string.Empty);
                        }
                        else
                        {
                            dataRow.CreateCell(16).SetCellValue(string.Empty);
                            dataRow.CreateCell(17).SetCellValue(string.Empty);
                            dataRow.CreateCell(18).SetCellValue(string.Empty);
                            dataRow.CreateCell(19).SetCellValue(string.Empty);
                            dataRow.CreateCell(20).SetCellValue(string.Empty);
                            dataRow.CreateCell(21).SetCellValue(string.Empty);
                        }

                        // hotel information
                        dataRow.CreateCell(22).SetCellValue(person.HotelConfirmation);
                        dataRow.CreateCell(23).SetCellValue(person.HotelCheckIn.HasValue ? person.HotelCheckIn.Value.ToString("d") : string.Empty);
                        dataRow.CreateCell(24).SetCellValue(person.HotelCheckOut.HasValue ? person.HotelCheckOut.Value.ToString("d") : string.Empty);
                    }
                    catch (Exception)
                    {
                        //do nothing
                    }


                }

                // Forcing formula recalculation...
                sheet.ForceFormulaRecalculation = true;

                var ms = new MemoryStream();

                // Writing the workbook content to the FileStream...
                templateWorkbook.Write(ms);

                // Sending the server processed data back to the user computer...
                //return File(ms.ToArray(), "application/vnd.ms-excel", fileName);
                //HttpContext.Session["Passwords"] = ms.ToArray();

                return File(ms.ToArray(), "application/vnd.ms-excel", string.Format("{0}-Attendees.xls", seminar.Year));
            }
            catch (Exception ex)
            {
                
            }

            return File(new byte[0], "application/vnd.mx-excel", "nothing.xls");
        }
    }
}
