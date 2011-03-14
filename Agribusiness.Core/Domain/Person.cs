using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Person : DomainObject
    {
        #region Constructors
        public Person() { SetDefaults(); }

        public Person(string lastName, string firstName, string phone)
        {
            LastName = lastName;
            FirstName = firstName;
            Phone = phone;

            SetDefaults();
        }

        private void SetDefaults()
        {
            Invite = false;

            Addresses = new List<Address>();
            Contacts = new List<Contact>();
            CaseStudyExecutive =  new List<CaseStudy>();
            CaseStudyAuthor = new List<CaseStudy>();
        }
        #endregion

        #region Mapped Fields
        // required fields
        [Required]
        [StringLength(50)]
        public virtual string LastName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string FirstName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public virtual string Phone { get; set; }
        /// <summary>
        /// Link to the asp.net membership user
        /// </summary>
        public virtual Guid UserId { get; set; }

        // optional fields
        [StringLength(50)]
        public virtual string MI { get; set; }
        [StringLength(5)]
        public virtual string Salutation { get; set; }
        [StringLength(50)]
        public virtual string BadgeName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public virtual string CellPhone { get; set; }
        [DataType(DataType.PhoneNumber)]
        public virtual string Fax { get; set; }

        public virtual byte[] Picture { get; set; }
        public virtual string Biography { get; set; }
        public virtual bool Invite { get; set; }

        public virtual User User { get; set; }

        // bags
        public virtual IList<Address> Addresses { get; set; }
        public virtual IList<Contact> Contacts { get; set; }
        public virtual IList<CaseStudy> CaseStudyExecutive { get; set; }
        public virtual IList<CaseStudy> CaseStudyAuthor { get; set; }
        #endregion
    }

    public class PersonMap : ClassMap<Person>
    {
        public PersonMap()
        {
            Id(x => x.Id);

            Map(x => x.LastName);
            Map(x => x.FirstName);
            Map(x => x.Phone);
            Map(x => x.UserId);

            Map(x => x.MI);
            Map(x => x.Salutation);
            Map(x => x.BadgeName);
            Map(x => x.CellPhone);
            Map(x => x.Fax);
            Map(x => x.Picture);
            Map(x => x.Biography);
            Map(x => x.Invite);

            References(x => x.User);

            HasMany(a => a.Addresses).Inverse().Cascade.AllDeleteOrphan();
            HasMany(a => a.Contacts).Inverse().Cascade.AllDeleteOrphan();

            HasManyToMany(x => x.CaseStudyExecutive)
                .ParentKeyColumn("PersonId")
                .ChildKeyColumn("CaseStudyId")
                .Table("CaseStudyExecutives")
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.CaseStudyAuthor)
                .ParentKeyColumn("PersonId")
                .ChildKeyColumn("CaseStudyId")
                .Table("CaseStudyAuthors")
                .Cascade.SaveUpdate();
        }
    }
}
