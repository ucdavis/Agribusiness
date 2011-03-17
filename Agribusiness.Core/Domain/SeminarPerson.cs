using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class SeminarPerson : DomainObject
    {
        #region Constructors
        public SeminarPerson() { SetDefaults(); }

        public SeminarPerson(Seminar seminar, Person person, SeminarRole seminarRole)
        {
            Seminar = seminar;
            Person = person;
            SeminarRole = seminarRole;
        }

        private void SetDefaults()
        {
            Registered = false;
        }
        #endregion

        #region Mapped Fields
        [Required]
        public virtual Seminar Seminar { get; set; }
        [Required]
        public virtual Person Person { get; set; }
        [Required]
        public virtual SeminarRole SeminarRole { get; set; }

        public virtual string Title { get; set; }
        public virtual Guid FirmCode { get; set; }
        public virtual string CouponCode { get; set; }
        public virtual bool Registered { get; set; }

        public virtual string RegistrationCode { get; private set; }

        public virtual IList<Session> Sessions { get; set; }
        #endregion
    }

    public class SeminarPersonMap : ClassMap<SeminarPerson>
    {
        public SeminarPersonMap()
        {
            Id(x => x.Id);

            References(x => x.Seminar);
            References(x => x.Person);
            References(x => x.SeminarRole);

            Map(x => x.Title);
            Map(x => x.FirmCode);
            Map(x => x.CouponCode);
            Map(x => x.Registered);
            Map(x => x.RegistrationCode).ReadOnly();

            HasManyToMany(x => x.Sessions).ParentKeyColumn("SeminarPersonId")
                .ChildKeyColumn("SessionId")
                .Table("SeminarPeopleXSessions")
                .Cascade.SaveUpdate();
        }
    }
}
