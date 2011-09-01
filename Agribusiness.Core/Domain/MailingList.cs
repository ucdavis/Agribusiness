using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class MailingList : DomainObject
    {
        public MailingList()
        {
            SetDefaults();   
        }

        public MailingList(string name, Seminar seminar)
        {
            Name = name;
            Seminar = seminar;

            SetDefaults();
        }

        private void SetDefaults()
        {
            DateCreated = DateTime.Now;
            DateUpdated = DateTime.Now;

            People = new List<Person>();
        }

        public virtual string Name { get; set; }
        public virtual Seminar Seminar { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateUpdated { get; set; }

        public virtual IList<Person> People { get; set; }
    }

    public class MailingListMap : ClassMap<MailingList>
    {
        public MailingListMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            References(x => x.Seminar).Cascade.None();
            Map(x => x.DateCreated);
            Map(x => x.DateUpdated);

            HasManyToMany(x => x.People).Table("MailingListsXPeople").ParentKeyColumn("MailingListId").ChildKeyColumn(
                "PersonId").Cascade.SaveUpdate();
        }
    }
}
