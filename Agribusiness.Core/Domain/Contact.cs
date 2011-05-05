using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Contact : DomainObject
    {
        #region Constructors
        public Contact() { }

        public Contact(string firstName, string lastName, string phone, ContactType contactType, Person person)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            ContactType = contactType;
            Person = person;
        }
        #endregion

        #region Mapped Fields
        [Required]
        [StringLength(50)]
        [Display(Name="First Name")]
        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name="Last Name")]
        public virtual string LastName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public virtual string Phone { get; set; }
        [Email]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }

        [StringLength(10)]
        [Display(Name="Extension")]
        public virtual string Ext { get; set; }

        [Required]
        public virtual ContactType ContactType { get; set; }
        [Required]
        public virtual Person Person { get; set; }
        #endregion

        /// <summary>
        /// Checks to see if any of the fields has a value
        /// </summary>
        public virtual bool HasContact { 
            get
            {
                return !(
                        string.IsNullOrWhiteSpace(FirstName)
                     && string.IsNullOrWhiteSpace(LastName)
                     && string.IsNullOrWhiteSpace(Phone)
                     && string.IsNullOrWhiteSpace(Email)
                    );
            }
        }
    }

    public class ContactMap : ClassMap<Contact>
    {
        public ContactMap()
        {
            Id(x => x.Id);

            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Phone);
            Map(x => x.Email);
            Map(x => x.Ext);
            References(x => x.ContactType);
            References(x => x.Person);
        }
    }
}
