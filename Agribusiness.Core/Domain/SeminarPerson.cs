﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;
using System.Linq;

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
            Paid = false;

            Sessions = new List<Session>();
            SeminarRoles = new List<SeminarRole>();
            Commodities = new List<Commodity>();
        }
        #endregion

        #region Mapped Fields
        [Required]
        public virtual Seminar Seminar { get; set; }
        [Required]
        public virtual Person Person { get; set; }
        [Required]
        [StringLength(200)]
        public virtual string Title { get; set; }
        [Required]
        public virtual Firm Firm { get; set; }
        public virtual string CouponCode { get; set; }
        [Range(0.0, Double.MaxValue)]
        public virtual decimal? CouponAmount { get; set; }
        public virtual bool Paid { get; set; }
        public virtual bool Invite { get; set; }
        
        public virtual string ReferenceId { get; protected set; }
        [StringLength(20)]
        public virtual string TransactionId { get; set; }
        public virtual string Comments { get; set; }

        [Display(Name="Check-In")]
        public virtual DateTime? HotelCheckIn { get; set; }
        [Display(Name="Check-Out")]
        public virtual DateTime? HotelCheckOut { get; set; }
        [StringLength(20)]
        [Display(Name="Confirmation")]
        public virtual string HotelConfirmation { get; set; }
        public virtual RoomType RoomType { get; set; }
        public virtual string HotelComments { get; set; }

        [Display(Name="Firm Type")]
        public virtual FirmType FirmType { get; set; }
        public virtual string OtherFirmType { get; set; }

        public virtual IList<Session> Sessions { get; set; }
        public virtual IList<SeminarRole> SeminarRoles { get; set; }
        public virtual IList<Commodity> Commodities { get; set; }
        #endregion

        public virtual string GetCommodityList()
        {
            return string.Join(", ", Commodities.Select(a=>a.Name));
        }

        /// <summary>
        /// Display Firm Type Name, based on firmtype field and otherfirmtype
        /// </summary>
        public virtual string FirmName { 
            get
            {
                return FirmType != null && FirmType.Name != "Other" ? FirmType.Name : (!string.IsNullOrWhiteSpace(OtherFirmType) ? OtherFirmType : "n/a");
            }
        }

    }

    public class SeminarPersonMap : ClassMap<SeminarPerson>
    {
        public SeminarPersonMap()
        {
            Id(x => x.Id);

            References(x => x.Seminar);
            References(x => x.Person);

            Map(x => x.Title);
            References(x => x.Firm).Cascade.All().Fetch.Join();
            Map(x => x.CouponCode);
            Map(x => x.CouponAmount);
            Map(x => x.Paid);
            Map(x => x.ReferenceId).ReadOnly();
            Map(x => x.Invite);
            
            Map(x => x.TransactionId);
            Map(x => x.Comments);

            Map(x => x.HotelCheckIn);
            Map(x => x.HotelCheckOut);
            Map(x => x.HotelConfirmation);
            References(x => x.RoomType);
            Map(x => x.HotelComments);

            References(x => x.FirmType);
            Map(x => x.OtherFirmType);

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
