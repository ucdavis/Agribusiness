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
        [StringLength(100)]
        public virtual string Name { get; set; }
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
        [StringLength(50)]
        public virtual string Location { get; set; }

        public virtual DateTime SubmittedDateTime { get; set; }
        public virtual bool Responded { get; set; }

        [Required]
        public virtual Seminar Seminar { get; set; }
        [Required]
        public virtual Site Site { get; set; }

        public virtual IEnumerable<InformationRequestNote> InformationRequestNotes { get; set; }
    }

    public class InformationRequestMap : ClassMap<InformationRequest>
    {
        public InformationRequestMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Title);
            Map(x => x.Company);
            Map(x => x.Email);
            Map(x => x.Commodity);
            Map(x => x.Location);
            Map(x => x.SubmittedDateTime);
            Map(x => x.Responded);

            References(x => x.Seminar);
            References(x => x.Site);

            HasMany(a => a.InformationRequestNotes).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
