using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Agribusiness.Core.Extensions;
using DataAnnotationsExtensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public enum ContactOption { Directly = 0, Assistant, CCAssistant }

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

            ContactAssistant = false;
            CarbonCopyAssistant = false;

            Commodities = new List<Commodity>();
        }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public virtual string FirstName { get; set; }
        [StringLength(50)]
        public virtual string MI { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }
        [StringLength(50)]
        [Display(Name = "Badge Name")]
        public virtual string BadgeName { get; set; }
        public virtual byte[] Photo { get; set; }
        public virtual string ContentType { get; set; }
        [StringLength(100)]
        [Display(Name = "Assistant Name")]
        public virtual string AssistantName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Assistant Phone")]
        public virtual string AssistantPhone { get; set; }
        [DataType(DataType.EmailAddress)]
        [Email]
        [Display(Name ="Assistant Email")]
        public virtual string AssistantEmail { get; set; }
        public virtual string Expectations { get; set; }

        /// <summary>
        /// Populated if an existing firm has been selected
        /// </summary>
        [EitherOr(new string[]{"FirmName", "FirmDescription"})]
        public virtual Firm Firm { get; set; }
        [StringLength(200)]
        [Display(Name = "Firm Name")]
        public virtual string FirmName { get; set; }
        [Display(Name = "Business Type/Firm Description")]
        public virtual string FirmDescription { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Address Line 1")]
        public virtual string FirmAddressLine1 { get; set; }
        [StringLength(100)]
        [Display(Name = "Address Line 2")]
        public virtual string FirmAddressLine2 { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "City")]
        public virtual string FirmCity { get; set; }
        [Required]
        [Display(Name="State")]
        [StringLength(50)]
        public virtual string FirmState { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name="Zip")]
        public virtual string FirmZip { get; set; }
        [Required]
        public virtual Country Country { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public virtual string FirmPhone { get; set; }
        [StringLength(10)]
        public virtual string FirmPhoneExt { get; set; }
        [DataType(DataType.Url)]
        [Url]
        [StringLength(200)]
        public virtual string Website { get; set; }
        [Required]
        [Display(Name = "Job Responsibilities")]
        public virtual string Responsibilities { get; set; }
        [StringLength(50)]
        [Required]
        [Display(Name = "Job Title")]
        public virtual string JobTitle { get; set; }

        public virtual bool IsPending { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual DateTime DateSubmitted { get; set; }
        public virtual DateTime? DateDecision { get; set; }
        public virtual string DecisionReason { get; set; }

        /// <summary>
        /// Whether or not attendee has authorized release of personal contact information.
        /// </summary>
        public virtual bool ContactInformationRelease { get; set; }

        /// <summary>
        /// Whether or not to contact the assistant only
        /// </summary>
        public virtual bool ContactAssistant { get; set; }
        /// <summary>
        /// Whether or not to cc assistant on all communications but contact directly
        /// </summary>
        public virtual bool CarbonCopyAssistant { get; set; }
        /// <summary>
        /// Whether or not to only contact person directly, no assistant
        /// </summary>
        public virtual bool ContactDirectly { get; set; }

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
            Map(x => x.Photo).LazyLoad().CustomType("BinaryBlob");
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
            Map(x => x.FirmState);
            Map(x => x.FirmZip);
            References(x => x.Country).Column("FirmCountry");
            Map(x => x.FirmPhone);
            Map(x => x.FirmPhoneExt);
            Map(x => x.Website);
            Map(x => x.Responsibilities);
            Map(x => x.JobTitle);

            Map(x => x.IsPending);
            Map(x => x.IsApproved);
            Map(x => x.DateSubmitted);
            Map(x => x.DateDecision);
            Map(x => x.DecisionReason);

            Map(x => x.ContactAssistant);
            Map(x => x.CarbonCopyAssistant);
            Map(x => x.ContactDirectly).ReadOnly();

            References(x => x.User);
            References(x => x.Seminar);

            HasManyToMany(x => x.Commodities).ParentKeyColumn("ApplicationId")
                .ChildKeyColumn("CommodityId")
                .Table("ApplicationXCommodity")
                .Cascade.SaveUpdate();
        }
    }
}
