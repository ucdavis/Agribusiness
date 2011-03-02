using System.ComponentModel.DataAnnotations;
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
        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string LastName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public virtual string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }

        [Required]
        public virtual ContactType ContactType { get; set; }
        [Required]
        public virtual Person Person { get; set; }
        #endregion
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
            References(x => x.ContactType);
            References(x => x.Person);
        }
    }
}
