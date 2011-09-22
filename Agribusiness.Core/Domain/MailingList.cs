using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        [Required]
        public virtual Seminar Seminar { get; set; }
        public virtual DateTime DateCreated { get; set; }
        public virtual DateTime DateUpdated { get; set; }

        public virtual IList<Person> People { get; set; }

        public virtual void AddPerson(Person person)
        {
            if (!People.Contains(person))
            {
                People.Add(person);
            }
        }
    }

    public class MailingListMap : ClassMap<MailingList>
    {
        public MailingListMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
            References(x => x.Seminar).Cascade.None();
            Map(x => x.DateCreated);
            Map(x => x.DateUpdated);

            HasManyToMany(x => x.People).Table("MailingListsXPeople").ParentKeyColumn("MailingListId").ChildKeyColumn(
                "PersonId").Cascade.SaveUpdate();
        }
    }
}
