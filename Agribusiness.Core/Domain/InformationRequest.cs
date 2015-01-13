using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class InformationRequest : DomainObject
    {
        public InformationRequest()
        {
            SubmittedDateTime = DateTime.Now;
            Responded = false;

            InformationRequestNotes = new List<InformationRequestNote>();
        }

        [Required]
        [StringLength(50)]
        public virtual string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public virtual string LastName { get; set; }
        [Required]
        [StringLength(75)]
        public virtual string Title { get; set; }
        [Required]
        [StringLength(75)]
        public virtual string Company { get; set; }
        [Required]
        [StringLength(50)]
        [Email]
        [DataType(DataType.EmailAddress)]
        public virtual string Email { get; set; }
        [Required]
        [StringLength(200)]
        public virtual string Commodity { get; set; }
        [Required]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public virtual string Phone { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "Address Line 1")]
        public virtual string AddressLine1 { get; set; }
        [StringLength(100)]
        [Display(Name = "Address Line 2")]
        public virtual string AddressLine2 { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "City")]
        public virtual string City { get; set; }
        [Required]
        [Display(Name = "State")]
        [StringLength(50)]
        public virtual string State { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Zip")]
        public virtual string Zip { get; set; }
        [Required]
        public virtual Country Country { get; set; }

        [StringLength(50)]
        [Display(Name="Referred By")]
        public virtual string ReferredBy { get; set; }
        [StringLength(50)]
        [Display(Name="First Name")]
        public virtual string AssistantFirstName { get; set; }
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public virtual string AssistantLastName { get; set; }
        [StringLength(50)]
        [Display(Name = "Email")]
        public virtual string AssistantEmail { get; set; }
        [StringLength(20)]
        [Display(Name = "Phone")]
        public virtual string AssistantPhone { get; set; }

        public virtual DateTime SubmittedDateTime { get; set; }
        public virtual bool Responded { get; set; }

        [Required]
        public virtual Seminar Seminar { get; set; }
        [Required]
        public virtual Site Site { get; set; }

        public virtual IEnumerable<InformationRequestNote> InformationRequestNotes { get; set; }

        public virtual string FullName()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }
    }

    public class InformationRequestMap : ClassMap<InformationRequest>
    {
        public InformationRequestMap()
        {
            Id(x => x.Id);

            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Title);
            Map(x => x.Company);
            Map(x => x.Email);
            Map(x => x.Commodity);
            Map(x => x.SubmittedDateTime);
            Map(x => x.Responded);
            Map(x => x.Phone);
            Map(x => x.AddressLine1);
            Map(x => x.AddressLine2);
            Map(x => x.City);
            Map(x => x.State);
            Map(x => x.Zip);
            References(x => x.Country);

            Map(x => x.ReferredBy);
            Map(x => x.AssistantFirstName);
            Map(x => x.AssistantLastName);
            Map(x => x.AssistantEmail);
            Map(x => x.AssistantPhone);

            References(x => x.Seminar);
            References(x => x.Site);

            HasMany(a => a.InformationRequestNotes).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
