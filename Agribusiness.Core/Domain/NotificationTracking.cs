﻿using System;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class NotificationTracking : DomainObject
    {
        public NotificationTracking()
        {
            SetDefaults();
        }

        public NotificationTracking(string notifiedBy, NotificationMethod notificationMethod, NotificationType notificationType)
        {
            NotificationMethod = notificationMethod;
            NotificationType = notificationType;
            NotifiedBy = notifiedBy;

            SetDefaults();
        }

        private void SetDefaults()
        {
            DateTime = DateTime.Now;
        }

        public virtual Person Person { get; set; }
        public virtual Application Application { get; set; }
        [Required]
        [Display(Name="Method")]
        public virtual NotificationMethod NotificationMethod { get; set; }
        [Required]
        [Display(Name="Type")]
        public virtual NotificationType NotificationType { get; set; }
        [Required]
        [Display(Name="Seminar")]
        public virtual Seminar Seminar { get; set; }

        public virtual DateTime DateTime { get; set; }
        [Required]
        public virtual string NotifiedBy { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string Comments { get; set; }

        public virtual EmailQueue EmailQueue { get; set; }
    }

    public class NotificationTrackingMap : ClassMap<NotificationTracking>
    {
        public NotificationTrackingMap()
        {
            Table("NotificationTracking");

            Id(x => x.Id);

            References(x => x.Person);
            References(x => x.Application);
            References(x => x.NotificationMethod);
            References(x => x.NotificationType);
            References(x => x.Seminar);

            Map(x => x.DateTime);
            Map(x => x.NotifiedBy);

            Map(x => x.Comments);

            References(x => x.EmailQueue).Cascade.SaveUpdate();
        }
    }

    public class NotificationMethod : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
    }

    public class NotificationMethodMap : ClassMap<NotificationMethod>
    {
        public NotificationMethodMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
        }
    }

    public class NotificationType : DomainObjectWithTypedId<string>
    {
        public NotificationType()
        {
            Display = true;
        }

        public virtual string Name { get; set; }
        public virtual bool Display { get; set; }
        public virtual bool SendAll { get; set; }
    }

    public class NotificationTypeMap : ClassMap<NotificationType>
    {
        public NotificationTypeMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Display);
            Map(x => x.SendAll);
        }
    }
}
