﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agribusiness.Core.Extensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Application : DomainObject
    {
        public Application()
        {
            SetDefault();
        }

        private void SetDefault()
        {
            IsPending = true;
            IsApproved = false;
            DateSubmitted = DateTime.Now;

            Commodities = new List<Commodity>();
        }

        [Required]
        [StringLength(50)]
        public virtual string FirstName { get; set; }
        [StringLength(50)]
        public virtual string MI { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string LastName { get; set; }
        [StringLength(50)]
        public virtual string BadgeName { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual string ContentType { get; set; }
        [StringLength(100)]
        public virtual string AssistantName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public virtual string AssistantPhone { get; set; }
        [DataType(DataType.EmailAddress)]
        public virtual string AssistantEmail { get; set; }
        public virtual string Expectations { get; set; }

        /// <summary>
        /// Populated if an existing firm has been selected
        /// </summary>
        [EitherOr(new string[]{"FirmName", "FirmDescription"})]
        public virtual Firm Firm { get; set; }
        [StringLength(200)]
        public virtual string FirmName { get; set; }
        public virtual string FirmDescription { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string FirmAddressLine1 { get; set; }
        [StringLength(100)]
        public virtual string FirmAddressLine2 { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string FirmCity { get; set; }
        [Required]
        public virtual State FirmState { get; set; }
        [Required]
        [StringLength(10)]
        public virtual string FirmZip { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public virtual string FirmPhone { get; set; }
        [DataType(DataType.Url)]
        [StringLength(200)]
        public virtual string Website { get; set; }
        public virtual string Responsibilities { get; set; }
        [StringLength(50)]
        public virtual string JobTitle { get; set; }

        public virtual bool IsPending { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual DateTime DateSubmitted { get; set; }
        public virtual DateTime? DateDecision { get; set; }

        [Required]
        public virtual User User { get; set; }
        [Required]
        public virtual Seminar Seminar { get; set; }

        public virtual IList<Commodity> Commodities { get; set; }


        #region Calculated Fields

        public virtual string FullName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MI))
                {
                    return string.Format("{0} {1}", FirstName, LastName);
                }

                return string.Format("{0} {1} {2}", FirstName, MI, LastName);
            }
        }
        #endregion
    }

    public class ApplicationMap : ClassMap<Application>
    {
        public ApplicationMap()
        {
            Id(x => x.Id);

            Map(x => x.FirstName);
            Map(x => x.MI);
            Map(x => x.LastName);
            Map(x => x.BadgeName);
            Map(x => x.Photo);
            Map(x => x.ContentType);
            Map(x => x.AssistantName);
            Map(x => x.AssistantPhone);
            Map(x => x.AssistantEmail);
            Map(x => x.Expectations);

            References(x => x.Firm);
            Map(x => x.FirmName);
            Map(x => x.FirmDescription);
            Map(x => x.FirmAddressLine1);
            Map(x => x.FirmAddressLine2);
            Map(x => x.FirmCity);
            References(x => x.FirmState).Column("FirmState");
            Map(x => x.FirmZip);
            Map(x => x.FirmPhone);
            Map(x => x.Website);
            Map(x => x.Responsibilities);
            Map(x => x.JobTitle);

            Map(x => x.IsPending);
            Map(x => x.IsApproved);
            Map(x => x.DateSubmitted);
            Map(x => x.DateDecision);

            References(x => x.User);
            References(x => x.Seminar);

            HasManyToMany(x => x.Commodities).ParentKeyColumn("ApplicationId")
                .ChildKeyColumn("CommodityId")
                .Table("ApplicationXCommodity")
                .Cascade.SaveUpdate();
        }
    }
}