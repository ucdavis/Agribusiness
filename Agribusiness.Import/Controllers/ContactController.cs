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
    public class ContactController : ApplicationController
    {
        private readonly string _contactTable = "ContactTable";
        private readonly string _archiveContactTable = "ArchiveContactTable";

        public ActionResult Contacts()
        {
            var imported = Db.Trackings.Where(a => a.Name == _contactTable).Any();

            var contacts = new List<Contact>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/Contact.xls", imported, contacts, errors);

            if (!imported)
            {
                var tracking = new Tracking() { Name = _contactTable };
                Db.Trackings.Add(tracking);
                Db.SaveChanges();
            }

            var viewModel = ContactViewModel.Create(contacts, errors, imported);
            return View(viewModel);
        }

        public ActionResult ArchiveContacts()
        {
            var imported = Db.Trackings.Where(a => a.Name == _archiveContactTable).Any();

            var contacts = new List<Contact>();
            var errors = new List<KeyValuePair<string, string>>();

            ReadData("~/Assets/ArchiveContact.xls", imported, contacts, errors);

            if (!imported)
            {
                var tracking = new Tracking() { Name = _archiveContactTable };
                Db.Trackings.Add(tracking);
                Db.SaveChanges();
            }

            var viewModel = ContactViewModel.Create(contacts, errors, imported);
            return View(viewModel);
        }

        private void ReadData(string file, bool imported, List<Contact> contacts, List<KeyValuePair<string, string>> errors)
        {
            contacts = new List<Contact>();
            errors = new List<KeyValuePair<string, string>>();

            var sheet = ExcelHelpers.OpenWorkbook(Server.MapPath(file));

            for (var i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);

                try
                {
                    var c_id = ExcelHelpers.ReadIntCell(row, -1);

                    // check the existance
                    if (Db.Contacts.Any(a => a.c_Id == c_id)) throw new Exception("Already exists");

                    var contact = new Contact();

                    contact.Badge =             ExcelHelpers.ReadCell(row, 0);      // a
                    contact.CourierCity =       ExcelHelpers.ReadCell(row, 1);      // b
                    contact.CourierCountry =    ExcelHelpers.ReadCell(row, 2);      // c
                    contact.CourierState =      ExcelHelpers.ReadCell(row, 3);      // d
                    contact.CourierStreet =     ExcelHelpers.ReadCell(row, 4);      // e
                    contact.CourierZip =        ExcelHelpers.ReadCell(row, 5);      // f
                    
                    contact.DeliveryCity =      ExcelHelpers.ReadCell(row, 6);      // g
                    contact.DeliveryCountry =   ExcelHelpers.ReadCell(row, 7);      // h
                    contact.DeliveryState =     ExcelHelpers.ReadCell(row, 8);      // i
                    contact.DeliveryStreet =    ExcelHelpers.ReadCell(row, 9);      // j
                    contact.DeliveryZip =       ExcelHelpers.ReadCell(row, 10);     // k

                    contact.HomeCity =          ExcelHelpers.ReadCell(row, 11);     // l
                    contact.HomeCountry =       ExcelHelpers.ReadCell(row, 12);     // m
                    contact.HomePreferred =     ExcelHelpers.ReadBoolCell(row, 13); // n
                    contact.HomeState =         ExcelHelpers.ReadCell(row, 14);     // o
                    contact.HomeStreet =        ExcelHelpers.ReadCell(row, 15);     // p
                    contact.HomeZip =           ExcelHelpers.ReadCell(row, 16);     // q

                    contact.ReportCity =        ExcelHelpers.ReadCell(row, 17);     // r
                    contact.ReportCountry =     ExcelHelpers.ReadCell(row, 18);     // s
                    contact.ReportFirm =        ExcelHelpers.ReadCell(row, 19);     // t
                    contact.ReportState =       ExcelHelpers.ReadCell(row, 20);     // u
                    contact.ReportStreet =      ExcelHelpers.ReadCell(row, 21);     // v
                    contact.ReportZip =         ExcelHelpers.ReadCell(row, 22);     // w

                    contact.AssistantEmail =    ExcelHelpers.ReadCell(row, 23);     // x
                    contact.AssistantEmailPreferred = ExcelHelpers.ReadBoolCell(row, 24);   // y
                    contact.AssistantEmailPreferredCc = ExcelHelpers.ReadBoolCell(row, 25); // z
                    contact.AssistantName =     ExcelHelpers.ReadCell(row, 26);     // aa
                    contact.AssistantPhone =    ExcelHelpers.ReadCell(row, 27);     // ab
                    contact.AssistantExt =      ExcelHelpers.ReadCell(row, 28);     // ac

                    contact.Backup =            ExcelHelpers.ReadCell(row, 29);     // ad
                    contact.Biography =         ExcelHelpers.ReadCell(row, 30);     // ae

                    contact.CellPhone =         ExcelHelpers.ReadCell(row, 31);     // af
                    contact.Email =             ExcelHelpers.ReadCell(row, 32);     // ag
                    contact.Fax =               ExcelHelpers.ReadCell(row, 33);     // ah
                    contact.FaxDelivery =       ExcelHelpers.ReadCell(row, 34);     // ai
                    contact.Phone =             ExcelHelpers.ReadCell(row, 35);     // aj
                    contact.PhoneDelivery =     ExcelHelpers.ReadCell(row, 36);     // ak
                    contact.Ext =               ExcelHelpers.ReadCell(row, 37);     // al

                    contact.DateCreated =       ExcelHelpers.ReadDateCell(row, 38); // am
                    contact.CurrentSeminarYear = ExcelHelpers.ReadIntCell(row, 39); // an
                    contact.CurrentYear =       ExcelHelpers.ReadIntCell(row, 40);  // ao
                    contact.DateLastUpdated =   ExcelHelpers.ReadDateCell(row, 41); // ap
                    contact.DateModified =      ExcelHelpers.ReadDateCell(row, 42); // aq

                    contact.EmergencyName =     ExcelHelpers.ReadCell(row, 43);     // ar
                    contact.EmergencyPhone =    ExcelHelpers.ReadCell(row, 44);     // as
                    contact.EmergencyExt =      ExcelHelpers.ReadCell(row, 45);     // at

                    contact.CommodityByFirmId = ExcelHelpers.ReadIntCell(row, 46);  // au
                    contact.CommodityByRContactFirmId = ExcelHelpers.ReadIntCell(row, 47);  // av
                    contact.c_Id =              ExcelHelpers.ReadIntCell(row, 48);  // aw
                    contact.LayoutFirmName =    ExcelHelpers.ReadCell(row, 49);     // ax
                    contact.MiscCopyAndPaste =  ExcelHelpers.ReadCell(row, 50);     // ay
                    
                    contact.FirstName =         ExcelHelpers.ReadCell(row, 51);     // az
                    contact.LastName =          ExcelHelpers.ReadCell(row, 52);     // ba
                    contact.MI =                ExcelHelpers.ReadCell(row, 53);     // bb
                    contact.Salutation =        ExcelHelpers.ReadCell(row, 54);     // bc
                    
                    contact.Notes =             ExcelHelpers.ReadCell(row, 55);     // bd

                    contact.CreatedBy =         ExcelHelpers.ReadCell(row, 56);     // be
                    contact.UpdatedBy =         ExcelHelpers.ReadCell(row, 57);     // bf
                    contact.ModifiedBy =        ExcelHelpers.ReadCell(row, 58);     // bg

                    contact.SpecialPreferences = ExcelHelpers.ReadCell(row, 59);    // bh

                    contact.CurrentYearAccepted = ExcelHelpers.ReadBoolCell(row, 60);   // bi
                    contact.CurrentYearInvitee = ExcelHelpers.ReadBoolCell(row, 61);// bj
                    contact.HasPhoto =          ExcelHelpers.ReadBoolCell(row, 62); // bk
                    contact.HasPhoto2 =         ExcelHelpers.ReadBoolCell(row, 63); // bl
                    contact.LoginName =         ExcelHelpers.ReadCell(row, 64);     // bm
                    contact.LoginName2 =        ExcelHelpers.ReadCell(row, 65);     // bn
                    contact.Password =          ExcelHelpers.ReadCell(row, 66);     // bo

                    contacts.Add(contact);

                    if (!imported)
                    {
                        Db.Contacts.Add(contact);
                    }

                }
                catch (Exception ex)
                {
                    errors.Add(new KeyValuePair<string, string>(ExcelHelpers.ReadIntCell(row, -1).ToString(), ex.Message));
                }

            }

            Db.SaveChanges();
        }
    }
}
