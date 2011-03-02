﻿using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Address : DomainObject
    {
        #region Constructors
        public Address() { }

        public Address(string line1, string line2, string city, State state, string zip, AddressType addressType, Person person)
        {
            Line1 = line1;
            // if this is blank, make it null
            Line2 = !string.IsNullOrEmpty(Line2) ? line2 : null;
            City = city;
            State = state;
            Zip = zip;
            AddressType = addressType;
            Person = person;
        }
        #endregion

        #region Mapped Fields
        [Required]
        [StringLength(100)]
        public virtual string Line1 { get; set; }
        [StringLength(100)]
        public virtual string Line2 { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string City { get; set; }
        [Required]
        [StringLength(2)]
        public virtual State State { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string Zip { get; set; }

        [Required]
        public virtual AddressType AddressType { get; set; }
        [Required]
        public virtual Person Person { get; set; }
        #endregion
    }

    public class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Id(x => x.Id);

            Map(x => x.Line1);
            Map(x => x.Line2);
            Map(x => x.City);
            References(x => x.State).Column("State");
            Map(x => x.Zip);

            References(x => x.AddressType);
            References(x => x.Person);
        }
    }
}
