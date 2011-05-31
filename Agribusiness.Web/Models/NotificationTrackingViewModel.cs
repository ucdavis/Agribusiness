using System;
using System.Collections.Generic;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class NotificationTrackingViewModel
    {
        public virtual NotificationTracking NotificationTracking { get; set; }
        public virtual IList<NotificationMethod> NotificationMethods { get; set; }
        public virtual IList<NotificationType> NotificationTypes { get; set; }

        public int PersonId { get; set; }
        public int SeminarId { get; set; }
        public Guid UserId { get; set; }

        public static NotificationTrackingViewModel Create(IRepository repository, int personId, int seminarId, Guid userId, NotificationTracking notificationTracking = null)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new NotificationTrackingViewModel(){
                                    NotificationTracking = notificationTracking ?? new NotificationTracking(),
                                    NotificationMethods = repository.OfType<NotificationMethod>().GetAll(), 
                                    NotificationTypes = repository.OfType<NotificationType>().GetAll(),
                                    PersonId = personId, SeminarId = seminarId, UserId = userId
                                };

            return viewModel;
        }
    }
}