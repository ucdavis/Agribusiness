using System;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.WS;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using INotificationService = Agribusiness.Web.Services.INotificationService;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Sync class
    /// </summary>
    [IpFilter]
    public class SyncController : ApplicationController
    {
        private readonly IRegistrationService _registrationService;
        private readonly INotificationService _notificationService;
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;

        public SyncController(IRegistrationService registrationService, INotificationService notificationService, IRepository<SeminarPerson> seminarPersonRepository )
        {
            _registrationService = registrationService;
            _notificationService = notificationService;
            _seminarPersonRepository = seminarPersonRepository;
        }

        public ActionResult Index()
        {
            try
            {
                var seminar = SiteService.GetLatestSeminar(Site);

                if (seminar.RegistrationId.HasValue)
                {
                    var result = _registrationService.RefreshAllRegistration(seminar.RegistrationId.Value);

                    // load all of the seminar people
                    var seminarPeople = seminar.SeminarPeople;

                    foreach (var sp in seminarPeople)
                    {
                        var reg = result.FirstOrDefault(a => a.ReferenceId == sp.ReferenceId);

                        if (reg != null)
                        {
                            sp.TransactionId = reg.TransactionId;
                            sp.Paid = reg.Paid;

                            // remove from the payment reminder mailing lists
                            if (sp.Paid)
                            {
                                _notificationService.RemoveFromMailingList(sp.Seminar, sp.Person, MailingLists.PaymentReminder);
                                _notificationService.RemoveFromMailingList(sp.Seminar, sp.Person, MailingLists.Registered);
                                _notificationService.AddToMailingList(sp.Seminar, sp.Person, MailingLists.Attending);
                            }

                            _seminarPersonRepository.EnsurePersistent(sp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var client = new SmtpClient("smtp.ucdavis.edu");
                var message = new MailMessage("automatedmessage@caes.ucdavis.edu", "anlai@ucdavis.edu");
                message.Subject = "Error Syncing from Agribusiness";
                message.Body = ex.Message;
                client.Send(message);
            }

            //if (ConfigurationManager.AppSettings["SyncCall"] == "true")
            //{
            //    // determine the address of the calling person.
            //    var client = new SmtpClient("smtp.ucdavis.edu");
            //    var message = new MailMessage("automatedmessage@caes.ucdavis.edu", "anlai@ucdavis.edu");
            //    message.Subject = "sync call was made to agribusiness and completed";
            //    message.Body = Request.UserHostAddress ?? "No address";
            //    client.Send(message);
            //}

            return View();
        }

    }
}
