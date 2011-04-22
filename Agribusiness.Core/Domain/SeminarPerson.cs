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

        public SeminarPerson(Seminar seminar, Person person)
        {
            Seminar = seminar;
            Person = person;
        }

        private void SetDefaults()
        {
            Registered = false;

            Sessions = new List<Session>();
            SeminarRoles = new List<SeminarRole>();
        }
        #endregion

        #region Mapped Fields
        [Required]
        public virtual Seminar Seminar { get; set; }
        [Required]
        public virtual Person Person { get; set; }

        public virtual string Title { get; set; }
        public virtual Firm Firm { get; set; }
        public virtual string CouponCode { get; set; }
        public virtual bool Registered { get; set; }
        public virtual bool Invite { get; set; }
        public virtual bool ContactInformationRelease { get; set; }

        public virtual string RegistrationCode { get; private set; }

        public virtual IList<Session> Sessions { get; set; }
        public virtual IList<SeminarRole> SeminarRoles { get; set; }
        public virtual IList<Commodity> Commodities { get; set; }
        #endregion
    }

    public class SeminarPersonMap : ClassMap<SeminarPerson>
    {
        public SeminarPersonMap()
        {
            Id(x => x.Id);

            References(x => x.Seminar);
            References(x => x.Person);

            Map(x => x.Title);
            References(x => x.Firm);
            Map(x => x.CouponCode);
            Map(x => x.Registered);
            Map(x => x.RegistrationCode).ReadOnly();
            Map(x => x.Invite);
            Map(x => x.ContactInformationRelease);

            HasManyToMany(x => x.Sessions).ParentKeyColumn("SeminarPersonId")
                .ChildKeyColumn("SessionId")
                .Table("SeminarPeopleXSessions")
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.SeminarRoles).ParentKeyColumn("SeminarPersonId")
                .ChildKeyColumn("SeminarRoleId")
                .Table("SeminarPeopleXSeminarRoles")
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.Commodities).ParentKeyColumn("SeminarPersonId")
                .ChildKeyColumn("CommodityId")
                .Table("SeminarPeopleXCommodities")
                .Cascade.SaveUpdate();
        }
    }
}
