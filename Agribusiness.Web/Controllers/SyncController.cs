using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.WS;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;
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
        private readonly ISeminarService _seminarService;
        private readonly INotificationService _notificationService;
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;

        public SyncController(IRegistrationService registrationService, ISeminarService seminarService, INotificationService notificationService, IRepository<SeminarPerson> seminarPersonRepository )
        {
            _registrationService = registrationService;
            _seminarService = seminarService;
            _notificationService = notificationService;
            _seminarPersonRepository = seminarPersonRepository;
        }

        public ActionResult Index()
        {
            var seminar = _seminarService.GetCurrent();

            if (seminar.RegistrationId.HasValue)
            {
                var result = _registrationService.RefreshAllRegistration(seminar.RegistrationId.Value);

                // load all of the seminar people
                var seminarPeople = seminar.SeminarPeople;

                foreach (var sp in seminarPeople)
                {
                    var reg = result.Where(a => a.ReferenceId == sp.ReferenceId).FirstOrDefault();

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
            
            return View();
        }

    }
}
