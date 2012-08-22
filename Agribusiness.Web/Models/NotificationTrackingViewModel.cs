using System;
using System.Collections.Generic;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using System.Linq;

namespace Agribusiness.Web.Models
{
    public class NotificationTrackingViewModel
    {
        public virtual NotificationTracking NotificationTracking { get; set; }
        public virtual IList<NotificationMethod> NotificationMethods { get; set; }
        public virtual IList<NotificationType> NotificationTypes { get; set; }
        public IEnumerable<MailingList> MailingLists { get; set; }
        public MailingList MailingList { get; set; }

        public virtual Seminar Seminar { get; set; }

        /// <summary>
        /// List of selected people
        /// </summary>
        public virtual IList<Person> People { get; set; }
        /// <summary>
        /// List of all people in the current seminar
        /// </summary>
        public virtual IList<Person> AllPeople { get; set; }

        public static NotificationTrackingViewModel Create(IRepository repository, string siteId, NotificationTracking notificationTracking = null, Person person = null, MailingList mailingList = null)
        {
            Check.Require(repository != null, "Repository is required.");

            var seminar = SiteService.GetLatestSeminar(siteId);

            var viewModel = new NotificationTrackingViewModel(){
                                    NotificationTracking = notificationTracking ?? new NotificationTracking(),
                                    NotificationMethods = repository.OfType<NotificationMethod>().GetAll(), 
                                    NotificationTypes = repository.OfType<NotificationType>().GetAll(),
                                    People = new List<Person>(),
                                    AllPeople = SiteService.GetLatestSeminar(siteId).SeminarPeople.Select(a => a.Person).ToList(),//seminarService.GetCurrent().SeminarPeople.Select(a=>a.Person).ToList(),
                                    Seminar = seminar,
                                    MailingLists = seminar.MailingLists,
                                    MailingList = mailingList
                                };

            if (person != null) viewModel.People.Add(person);

            return viewModel;
        }
    }

    public class SendNotificationViewModel
    {
        public NotificationTrackingViewModel NotificationTrackingViewModel { get; set; }
        public EmailQueue EmailQueue { get; set; }

        public static SendNotificationViewModel Create(IRepository repository, NotificationTrackingViewModel notificationTrackingViewModel, EmailQueue emailQueue = null, string email = null)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(notificationTrackingViewModel != null, "notificationTrackingViewModel is required.");

            var viewModel = new SendNotificationViewModel()
                                {
                                    NotificationTrackingViewModel = notificationTrackingViewModel,
                                    EmailQueue = emailQueue ?? new EmailQueue() { FromAddress = email}
                                };

            return viewModel;
        }
    }
}