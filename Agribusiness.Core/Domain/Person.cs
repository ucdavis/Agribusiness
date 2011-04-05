using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;
using System.Linq;

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

            SeminarPeople = new List<SeminarPerson>();
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
        
        public virtual string Biography { get; set; }
        public virtual bool Invite { get; set; }

        [Required]
        public virtual User User { get; set; }

        public virtual byte[] OriginalPicture { get; set; }
        public virtual byte[] MainProfilePicture { get; set; }
        public virtual byte[] ThumbnailPicture { get; set; }
        public virtual string ContentType { get; set; }

        #region Bags
        /// <summary>
        /// Gives a list of all the registration's a person has made
        /// </summary>
        //TODO: add a constraint that there must be at least one seminar person
        public virtual IList<SeminarPerson> SeminarPeople { get; set; }
        public virtual IList<Address> Addresses { get; set; }
        public virtual IList<Contact> Contacts { get; set; }
        public virtual IList<CaseStudy> CaseStudyExecutive { get; set; }
        public virtual IList<CaseStudy> CaseStudyAuthor { get; set; }
        #endregion
        #endregion

        #region Calculated Fields and Methods
        public virtual string FullName
        {
            get
            {
                // return full name
                if (!string.IsNullOrWhiteSpace(MI))
                    return string.Format("{0} {1} {2}", FirstName, MI, LastName);

                // just return first and last name
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        public virtual void AddAddress(Address address)
        {
            address.Person = this;

            Addresses.Add(address);
        }

        public virtual void AddContact(Contact contact)
        {
            contact.Person = this;

            Contacts.Add(contact);
        }

        public virtual void AddSeminarPerson(SeminarPerson seminarPerson)
        {
            seminarPerson.Person = this;

            SeminarPeople.Add(seminarPerson);
        }

        public virtual SeminarPerson GetLatestRegistration()
        {
            return SeminarPeople.AsQueryable().LastOrDefault();
        }
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
            
            Map(x => x.MI);
            Map(x => x.Salutation);
            Map(x => x.BadgeName);
            Map(x => x.CellPhone);
            Map(x => x.Fax);
            Map(x => x.Biography);
            Map(x => x.Invite);

            Map(x => x.OriginalPicture).LazyLoad().CustomType("BinaryBlob");
            Map(x => x.MainProfilePicture).LazyLoad().CustomType("BinaryBlob");
            Map(x => x.ThumbnailPicture).LazyLoad().CustomType("BinaryBlob");
            Map(x => x.ContentType).LazyLoad();

            References(x => x.User);

            HasMany(x => x.SeminarPeople).Inverse().Cascade.AllDeleteOrphan();
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
