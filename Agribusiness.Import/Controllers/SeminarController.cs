using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Import.Helpers;
using Agribusiness.Import.Models;
using Agribusiness.Import.Models.View;

namespace Agribusiness.Import.Controllers
{
    public class SeminarController : ApplicationController
    {
        private readonly string _seminarTable = "SeminarTable";
        private readonly string _ArchiveSeminarTable = "ArchiveSeminarTable";

        public ActionResult Seminar()
        {
            var imported = Db.Trackings.Where(a => a.Name == _seminarTable).Any();

            var seminars = new List<Seminar>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/Seminar.xls", imported, seminars, errors);

            if (!imported)
            {
                var tracking = new Tracking() { Name = _seminarTable };
                Db.Trackings.Add(tracking);
                Db.SaveChanges();
            }

            var viewModel = SeminarViewModel.Create(seminars, errors, imported);
            return View(viewModel);
        }

        private void ReadData(string file, bool imported, List<Seminar> seminars, List<KeyValuePair<string, string>> errors)
        {
            seminars = new List<Seminar>();
            errors = new List<KeyValuePair<string, string>>();

            var sheet = ExcelHelpers.OpenWorkbook(Server.MapPath(file));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    var s_id = ExcelHelpers.ReadIntCell(row, 13);

                    // check the existance
                    if (Db.Seminars.Any(a => a.s_Id == s_id)) throw new Exception("Already exists");

                    var seminar = new Seminar();

                    seminar.ApplicationStatus = ExcelHelpers.ReadCell(row, 0);  // a
                    seminar.ContactName = ExcelHelpers.ReadCell(row, 1);        // b
                    seminar.DateApplicationFormComplete = ExcelHelpers.ReadDateCell(row, 2);    // c
                    seminar.DateCreated = ExcelHelpers.ReadDateCell(row, 3);    // d
                    seminar.DateModified = ExcelHelpers.ReadDateCell(row, 4);   // e
                    seminar.Expectations = ExcelHelpers.ReadCell(row, 5);       // f
                    seminar.ContactId = ExcelHelpers.ReadIntCell(row, 6);       // g

                    seminar.s_Id = ExcelHelpers.ReadIntCell(row, 7);            // h
                    seminar.CreatedBy = ExcelHelpers.ReadCell(row, 8);          // i
                    seminar.ModifiedBy = ExcelHelpers.ReadCell(row, 9);         // j

                    seminar.IsCaseExecutive = ExcelHelpers.ReadBoolCell(row, 10);   // k
                    seminar.IsDiscussionGroupLead = ExcelHelpers.ReadBoolCell(row, 11); // l
                    seminar.IsFaculty = ExcelHelpers.ReadBoolCell(row, 12);     // m
                    seminar.IsSteeringCommittee = ExcelHelpers.ReadBoolCell(row, 13);   // n
                    seminar.IsPanelist = ExcelHelpers.ReadBoolCell(row, 14);    // o
                    seminar.IsParticipant = ExcelHelpers.ReadBoolCell(row, 15); // p
                    seminar.IsSpeaker = ExcelHelpers.ReadBoolCell(row, 16);     // Q
                    seminar.IsStaff = ExcelHelpers.ReadBoolCell(row, 17);       // r
                    seminar.IsVendor = ExcelHelpers.ReadBoolCell(row, 18);      // s
                    seminar.Accepted = ExcelHelpers.ReadBoolCell(row, 19);      // t
                    seminar.ExpensesComped = ExcelHelpers.ReadBoolCell(row, 20);// u
                    seminar.Year = ExcelHelpers.ReadIntCell(row, 21);           // v
                    seminar.PreviousYear = ExcelHelpers.ReadIntCell(row, 22);   // w

                    seminars.Add(seminar);

                    if (!imported)
                    {
                        Db.Seminars.Add(seminar);
                    }

                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(ExcelHelpers.ReadIntCell(row, 13).ToString(), ex.Message));
                }

            }

            Db.SaveChanges();
        }
    }
}
