using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;

namespace Agribusiness.Web.Services
{
    public class EventService : IEventService
    {
        private readonly INotificationService _notificationService;
        private readonly ISeminarService _seminarService;

        public EventService(INotificationService notificationService, ISeminarService seminarService)
        {
            _notificationService = notificationService;
            _seminarService = seminarService;
        }

        public void Invite(Person person)
        {
            var seminar = _seminarService.GetCurrent();

            _notificationService.AddToMailingList(seminar, person, MailingLists.Invitation);
        }

        public void Apply(Person person)
        {
            var seminar = _seminarService.GetCurrent();

            // deal with the mailing list
            _notificationService.RemoveFromMailingList(seminar, person, MailingLists.Invitation);
            _notificationService.AddToMailingList(seminar, person, MailingLists.Applied);
        }

        public void Accepted(Person person)
        {
            var seminar = _seminarService.GetCurrent();

            _notificationService.AddToMailingList(seminar, person, MailingLists.Registered);

            // add user to the reminder emails
            _notificationService.AddToMailingList(seminar, person, MailingLists.PaymentReminder);
            _notificationService.AddToMailingList(seminar, person, MailingLists.HotelReminder);
            if (person.OriginalPicture == null) _notificationService.AddToMailingList(seminar, person, MailingLists.PhotoReminder);
            if (string.IsNullOrWhiteSpace(person.Biography)) _notificationService.AddToMailingList(seminar, person, MailingLists.BioReminder);
            _notificationService.RemoveFromMailingList(seminar, person, MailingLists.Applied);
        }

        public void Denied(Person person)
        {
            var seminar = _seminarService.GetCurrent();

            _notificationService.AddToMailingList(seminar, person, MailingLists.Denied);
            _notificationService.RemoveFromMailingList(seminar, person, MailingLists.Applied);
        }

        public void Paid(Person person)
        {
            var seminar = _seminarService.GetCurrent();
            var seminarPerson = person.GetLatestRegistration();

            if (seminarPerson.Paid)
            {
                _notificationService.RemoveFromMailingList(seminar, person, MailingLists.PaymentReminder);
                _notificationService.AddToMailingList(seminar, person, MailingLists.Attending);    
            }

            
        }

        public void HotelUpdate(Person person)
        {
            var seminar = _seminarService.GetCurrent();
            var seminarPerson = person.GetLatestRegistration();

            if (seminarPerson.Seminar == seminar && !string.IsNullOrWhiteSpace(seminarPerson.HotelConfirmation))
            {
                _notificationService.RemoveFromMailingList(seminar, person, MailingLists.HotelReminder);
            }
        }

        public void PhotoUpdate(Person person)
        {
            if (person.OriginalPicture != null)
            {
                var seminar = _seminarService.GetCurrent();
                _notificationService.RemoveFromMailingList(seminar, person, MailingLists.PhotoReminder);
            }
        }

        public void BioUpdate(Person person)
        {
            if (!string.IsNullOrWhiteSpace(person.Biography))
            {
                var seminar = _seminarService.GetCurrent();
                _notificationService.RemoveFromMailingList(seminar, person, MailingLists.BioReminder);
            }
        }
    }
}