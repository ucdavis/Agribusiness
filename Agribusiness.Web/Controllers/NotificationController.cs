using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using AutoMapper;
using NPOI.HSSF.UserModel;
using Resources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.Attributes;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Notification class
    /// </summary>
    [UserOnly]
    public class NotificationController : ApplicationController
    {
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepository<NotificationTracking> _notificationTrackingRepository;
        private readonly IRepositoryWithTypedId<NotificationMethod, string> _notificationMethodRepository;
        private readonly IRepository<Attachment> _attachmentRepository;
        private readonly INotificationService _notificationService;
        private readonly IPersonService _personService;

        public NotificationController(IRepository<Person> personRepository, IRepository<Seminar> seminarRepository, IRepository<NotificationTracking> notificationTrackingRepository
                                    , IRepositoryWithTypedId<NotificationMethod, string> notificationMethodRepository, IRepository<Attachment> attachmentRepository
                                    , INotificationService notificationService, IPersonService personService)
        {
            _personRepository = personRepository;
            _seminarRepository = seminarRepository;
            _notificationTrackingRepository = notificationTrackingRepository;
            _notificationMethodRepository = notificationMethodRepository;
            _attachmentRepository = attachmentRepository;
            _notificationService = notificationService;
            _personService = personService;
        }

        /// <summary>
        /// Action for adding a tracking object, for notifications sent outside of the program
        /// </summary>
        /// <param name="personId">If sending to a specific person</param>
        /// <returns></returns>
        public ActionResult Create(int? personId)
        {
            Person person = null;
            if (personId.HasValue)
            {
                person = _personRepository.GetNullableById(personId.Value);
            }

            var viewModel = NotificationTrackingViewModel.Create(Repository, Site, person: person);
            
            viewModel.NotificationTracking.NotifiedBy = CurrentUser.Identity.Name;
            viewModel.NotificationTracking.Seminar = SiteService.GetLatestSeminar(Site);
            
            return View(viewModel);
        }

        /// <summary>
        /// Create a notification tracking object
        /// </summary>
        /// <param name="seminarId"></param>
        /// <param name="notificationTracking"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(List<int> people, NotificationTracking notificationTracking)
        {
            if (people == null || people.Count <= 0)
            {
                ModelState.AddModelError("People", "No person has been selected to receive the notification.");
            }

            var tracking = new List<NotificationTracking>();

            if (ModelState.IsValid)
            {
                tracking = ProcessTracking(ModelState, people, notificationTracking, new int[0]);

                foreach (var a in tracking)
                {
                    _notificationTrackingRepository.EnsurePersistent(a);
                }
                
                Message = string.Format(Messages.Saved, "Notification tracking");

                if (tracking.Count == 1)
                {
                    var person = tracking[0].Person;

                    var url = Url.Action("AdminEdit", "Person", new { id = person.User.Id, seminarId = SiteService.GetLatestSeminar(Site).Id });
                    return Redirect(string.Format("{0}#notifications", url));
                }
                
                // redirect back to the seminar controller details
                return this.RedirectToAction<SeminarController>(a => a.Details(null));
            }

            var viewModel = NotificationTrackingViewModel.Create(Repository, Site, notificationTracking);
            viewModel.People = tracking.Select(a=>a.Person).ToList();

            return View(viewModel);
        }

        /// <summary>
        /// Sending a notification through this program
        /// </summary>
        /// <returns></returns>
        public ActionResult Send(int? personId)
        {
            Person person = null;
            if (personId.HasValue)
            {
                person = _personRepository.GetNullableById(personId.Value);
            }

            var ntViewModel = NotificationTrackingViewModel.Create(Repository, Site, person: person);

            ntViewModel.NotificationTracking.NotifiedBy = CurrentUser.Identity.Name;
            ntViewModel.NotificationTracking.Seminar = SiteService.GetLatestSeminar(Site);
            ntViewModel.NotificationTracking.NotificationMethod = _notificationMethodRepository.GetById(StaticIndexes.NotificationMethod_Email);

            var viewModel = SendNotificationViewModel.Create(Repository, ntViewModel);
            return View(viewModel);
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Send(List<int> people, NotificationTracking notificationTracking, EmailQueue emailQueue, int? mailingListId, int[] attachmentIds)
        {
            if ((people == null || people.Count <= 0) && !mailingListId.HasValue)
            {
                ModelState.AddModelError("People", "No person has been selected to receive the notification.");
            }

            var tracking = new List<NotificationTracking>();

            ModelState.Clear();
            notificationTracking.TransferValidationMessagesTo(ModelState);

            var mailingList = mailingListId.HasValue ? Repository.OfType<MailingList>().GetNullableById(mailingListId.Value) : null;

            // save the objects if we are good
            if (ModelState.IsValid)
            {
                tracking = ProcessTracking(ModelState, people, notificationTracking, attachmentIds, emailQueue, mailingList);

                foreach (var a in tracking)
                {
                    _notificationTrackingRepository.EnsurePersistent(a);
                }

                Message = string.Format(Messages.Saved, "Notification tracking");

                if (tracking.Count == 1 && !mailingListId.HasValue)
                {
                    var person = tracking[0].Person;

                    var url = Url.Action("AdminEdit", "Person", new { id = person.User.Id, seminarId = SiteService.GetLatestSeminar(Site).Id });
                    return Redirect(string.Format("{0}#notifications", url));
                }

                // redirect back to the seminar controller details
                return this.RedirectToAction<SeminarController>(a => a.Details(null));
            }

            // not good, hand the page back
            var ntViewModel = NotificationTrackingViewModel.Create(Repository, Site, notificationTracking, mailingList:mailingList );
            ntViewModel.People = tracking.Select(a => a.Person).ToList();

            var viewModel = SendNotificationViewModel.Create(Repository, ntViewModel, emailQueue);
            return View(viewModel);
        }
     
        [HttpPost]
        [BypassAntiForgeryToken]
        public JsonResult SaveAttachment()
        {
            try
            {

                var request = ControllerContext.HttpContext.Request;
                var fileName = request["qqfile"];
                var contentType = request.Headers["X-File-Type"];

                byte[] contents;

                using (var reader = new BinaryReader(request.InputStream))
                {
                    contents = reader.ReadBytes((int) request.InputStream.Length);
                }

                // save the attachment
                var attachment = new Attachment() {Contents = contents, ContentType = contentType, FileName = fileName};
                _attachmentRepository.EnsurePersistent(attachment);

                return Json(new {id = attachment.Id, fileName = fileName, success=true});
            }
            catch (Exception)
            {
                return Json(false);
            }

            return Json(false);
        }

        /// <summary>
        /// Create all the notification tracking objects and email queue if provided
        /// </summary>
        /// <param name="modelState"></param>
        /// <param name="people"></param>
        /// <param name="notificationTracking"></param>
        /// <param name="emailQueue">(Optional)</param>
        /// <returns></returns>
        protected List<NotificationTracking> ProcessTracking(ModelStateDictionary modelState, List<int> people, NotificationTracking notificationTracking, int[] attachmentIds, EmailQueue emailQueue = null, MailingList mailingList = null)
        {
            Check.Require(people != null || mailingList != null, "people is required.");
            Check.Require(notificationTracking != null, "notificationTracking is required.");

            mailingList = mailingList ?? new MailingList();
            people = people ?? new List<int>();

            // build the list of people to send to
            var peeps = _personRepository.Queryable.Where(a => people.Contains(a.Id)).ToList();
            peeps.AddRange(mailingList.People);

            var tracking = new List<NotificationTracking>();
            var passwords = new List<KeyValuePair<Person, string>>();
            var invitations = new List<Invitation>();
            if (mailingList.Name == MailingLists.Invitation)
            {
                passwords = _personService.ResetPasswords(peeps);
            }

            foreach (var person in peeps.Distinct())
            {
                var nt = new NotificationTracking();
                // copy the fields
                Mapper.Map(notificationTracking, nt);
                nt.Seminar = notificationTracking.Seminar;
                // assign the person
                nt.Person = person;

                if (emailQueue != null)
                {
                    var eq = new EmailQueue();

                    Mapper.Map(emailQueue, eq);

                    Invitation invitation = null;
                    string password = null;
                    if (mailingList.Name == MailingLists.Invitation)
                    {
                        // get the invitation object
                        invitation = Repository.OfType<Invitation>().Queryable.FirstOrDefault(a => a.Person == person && a.Seminar == notificationTracking.Seminar);
                        
                        invitations.Add(invitation);

                        // get the person object
                        password = passwords.Where(a => a.Key == person).Select(a=>a.Value).FirstOrDefault();
                    }

                    eq.Body = _notificationService.GenerateNotification(eq.Body, person, Site, notificationTracking.Seminar.Id, invitation, password);

                    if (attachmentIds != null)
                    {
                        // add attachments
                        var attachments = _attachmentRepository.Queryable.Where(a => attachmentIds.Contains(a.Id)).ToList();
                        foreach (var a in attachments) eq.Attachments.Add(a);                        
                    }
                    
                    eq.Person = person;
                    nt.EmailQueue = eq;
                }

                // add it to the list
                tracking.Add(nt);
            }

            // add errors for those not in the list
            foreach (var id in people.Where(x => !peeps.Select(a => a.Id).Contains(x)))
            {
                ModelState.AddModelError("Person", string.Format("Person with id {0} could not be found.", id));
            }

            if (mailingList.Name == MailingLists.Invitation) GeneratePasswordReport(passwords, invitations);

            return tracking;
        }

        private void GeneratePasswordReport(List<KeyValuePair<Person, string>> passwords, List<Invitation> invitations )
        {
             try
            {
                // Opening the Excel template...
                var fs = new FileStream(Server.MapPath(@"~\Content\NPOITemplate.xls"), FileMode.Open, FileAccess.Read);

                // Getting the complete workbook...
                var templateWorkbook = new HSSFWorkbook(fs, true);

                // Getting the worksheet by its name...
                var sheet = templateWorkbook.GetSheetAt(0);// GetSheet("Sheet1");

                // Getting the row... 0 is the first row. aka title row
                var dataRow = sheet.CreateRow(0);
                dataRow.CreateCell(0).SetCellValue("Last Name");
                dataRow.CreateCell(1).SetCellValue("First Name");
                dataRow.CreateCell(2).SetCellValue("User Name");
                dataRow.CreateCell(3).SetCellValue("Password");
                dataRow.CreateCell(4).SetCellValue("Title");
                dataRow.CreateCell(5).SetCellValue("Firm");
                dataRow.CreateCell(6).SetCellValue("Line 1");
                dataRow.CreateCell(7).SetCellValue("Line 2");
                dataRow.CreateCell(8).SetCellValue("City");
                dataRow.CreateCell(9).SetCellValue("State");
                dataRow.CreateCell(10).SetCellValue("Zip");
                dataRow.CreateCell(11).SetCellValue("Country");
 
                // go through ever record and write out the spreadsheet
                passwords = passwords.OrderBy(a => a.Key.LastName).ToList();

                for(var i = 0; i < passwords.Count; i++)
                {
                    var password = passwords[i];
                    dataRow = sheet.CreateRow(i + 1);

                    var invitation = invitations.FirstOrDefault(a => a.Person == password.Key);
                    var seminarPerson = password.Key.GetLatestRegistration();

                    // fill the data
                    dataRow.CreateCell(0).SetCellValue(password.Key.LastName);
                    dataRow.CreateCell(1).SetCellValue(password.Key.FirstName);
                    dataRow.CreateCell(2).SetCellValue(password.Key.User.LoweredUserName);
                    dataRow.CreateCell(3).SetCellValue(password.Value);
                    dataRow.CreateCell(4).SetCellValue(invitation != null ? invitation.Title : (seminarPerson != null ? seminarPerson.Title : "n/a"));
                    dataRow.CreateCell(5).SetCellValue(invitation != null ? invitation.FirmName : (seminarPerson != null ? seminarPerson.Firm.Name : "n/a"));

                    try
                    {
                        // try to get the courier address
                        var address = invitation.Person.Addresses.FirstOrDefault(a => a.AddressType.Id == 'C');
                        if (address == null) address = invitation.Person.Addresses.FirstOrDefault(a => a.AddressType.Id == 'B');

                        if (address != null)
                        {
                            dataRow.CreateCell(6).SetCellValue(address.Line1);
                            dataRow.CreateCell(7).SetCellValue(address.Line2);
                            dataRow.CreateCell(8).SetCellValue(address.City);
                            dataRow.CreateCell(9).SetCellValue(address.State);
                            dataRow.CreateCell(10).SetCellValue(address.Zip);
                            dataRow.CreateCell(11).SetCellValue(address.Country != null ? address.Country.Name : "n/a");
                        }
                        else
                        {
                            dataRow.CreateCell(6).SetCellValue("n/a");
                            dataRow.CreateCell(7).SetCellValue("n/a");
                            dataRow.CreateCell(8).SetCellValue("n/a");
                            dataRow.CreateCell(9).SetCellValue("n/a");
                            dataRow.CreateCell(10).SetCellValue("n/a");
                            dataRow.CreateCell(11).SetCellValue("n/a");
                        }
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


                ControllerContext.HttpContext.Session.Remove("Passwords");

                ControllerContext.HttpContext.Session["Passwords"] = new KeyValuePair<DateTime, byte[]>(DateTime.Now, ms.ToArray());
            }
            catch (Exception ex)
            {
                throw;

                //Message = "Error Creating Excel Report " + ex.Message;

                //return this.RedirectToAction<HomeController>(a => a.AdminHome());
            }
        }

        public FileResult DownloadPasswordFile()
        {
            if (ControllerContext.HttpContext.Session["Passwords"] != null)
            {
                var contents = (KeyValuePair<DateTime, byte[]>)ControllerContext.HttpContext.Session["Passwords"];

                if (contents.Key.AddMinutes(10) > DateTime.Now)
                {
                    ControllerContext.HttpContext.Session.Remove("Passwords");
                    return File(contents.Value, "application/vnd.ms-excel", "AgbizPasswords.xls");
                }

            }

            ControllerContext.HttpContext.Session.Remove("Passwords");
            return File(new byte[0], "text/plain");
        }
    }
}