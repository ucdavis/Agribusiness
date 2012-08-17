using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;

namespace Agribusiness.Web.Services
{
    public class EventService : IEventService
    {
        private readonly INotificationService _notificationService;
        private readonly IPersonService _personService;

        public EventService(INotificationService notificationService, IPersonService personService)
        {
            _notificationService = notificationService;
            _personService = personService;
        }

        public void Invite(Person person, string siteId)
        {
            var seminar = SiteService.GetLatestSeminar(siteId);

            _notificationService.AddToMailingList(seminar, person, MailingLists.Invitation);
        }

        public void Apply(Person person, Application application, string siteId)
        {
            var seminar = SiteService.GetLatestSeminar(siteId);

            // deal with the mailing list
            _notificationService.RemoveFromMailingList(seminar, person, MailingLists.Invitation);
            _notificationService.AddToMailingList(seminar, person, MailingLists.Applied);

            // send email to confirm application
            _notificationService.GenerateConfirmation(application);

            // update the person's record
            _personService.UpdatePerson(person, application);
        }

        public void Accepted(Person person, string siteId)
        {
            var seminar = SiteService.GetLatestSeminar(siteId);

            _notificationService.AddToMailingList(seminar, person, MailingLists.Registered);

            // add user to the reminder emails
            _notificationService.AddToMailingList(seminar, person, MailingLists.PaymentReminder);
            _notificationService.AddToMailingList(seminar, person, MailingLists.HotelReminder);
            if (person.OriginalPicture == null) _notificationService.AddToMailingList(seminar, person, MailingLists.PhotoReminder);
            if (string.IsNullOrWhiteSpace(person.Biography)) _notificationService.AddToMailingList(seminar, person, MailingLists.BioReminder);
            _notificationService.RemoveFromMailingList(seminar, person, MailingLists.Applied);
        }

        public void Denied(Person person, string siteId)
        {
            var seminar = SiteService.GetLatestSeminar(siteId);

            _notificationService.AddToMailingList(seminar, person, MailingLists.Denied);
            _notificationService.RemoveFromMailingList(seminar, person, MailingLists.Applied);
        }

        public void Paid(Person person, string siteId)
        {
            var seminar = SiteService.GetLatestSeminar(siteId);
            var seminarPerson = person.GetLatestRegistration();

            if (seminarPerson.Paid)
            {
                _notificationService.RemoveFromMailingList(seminar, person, MailingLists.PaymentReminder);
                _notificationService.AddToMailingList(seminar, person, MailingLists.Attending);    
            }
        }

        public void HotelUpdate(Person person, string siteId)
        {
            var seminar = SiteService.GetLatestSeminar(siteId);
            var seminarPerson = person.GetLatestRegistration();

            if (seminarPerson.Seminar == seminar && !string.IsNullOrWhiteSpace(seminarPerson.HotelConfirmation))
            {
                _notificationService.RemoveFromMailingList(seminar, person, MailingLists.HotelReminder);
            }
        }

        public void PhotoUpdate(Person person, string siteId)
        {
            if (person.OriginalPicture != null)
            {
                var seminar = SiteService.GetLatestSeminar(siteId);
                _notificationService.RemoveFromMailingList(seminar, person, MailingLists.PhotoReminder);
            }
        }

        public void BioUpdate(Person person, string siteId)
        {
            if (!string.IsNullOrWhiteSpace(person.Biography))
            {
                var seminar = SiteService.GetLatestSeminar(siteId);
                _notificationService.RemoveFromMailingList(seminar, person, MailingLists.BioReminder);
            }
        }
    }
}