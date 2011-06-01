﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class EmailQueue : DomainObject
    {
        public EmailQueue()
        {
            SetDefaults();
        }

        public EmailQueue(Person person)
        {
            Person = person;

            SetDefaults();
        }

        private void SetDefaults()
        {
            Created = DateTime.Now;
            Pending = true;
        }

        [Required]
        public virtual Person Person { get; set; }

        public virtual DateTime Created { get; set; }
        public virtual bool Pending { get; set; }
        
        public virtual DateTime? SentDateTime { get; set; }

        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }

        public virtual int? ErrorCode { get; set; }
    }

    public class EmailQueueMap : ClassMap<EmailQueue>
    {
        public EmailQueueMap()
        {
            Table("EmailQueue");

            Id(x => x.Id);

            References(x => x.Person);

            Map(x => x.Created);
            Map(x => x.Pending);
            Map(x => x.SentDateTime);
            Map(x => x.Subject);
            Map(x => x.Body);
            Map(x => x.ErrorCode);
        }
    }
}
