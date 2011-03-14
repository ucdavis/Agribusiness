using System;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Seminar : DomainObject
    {
        public Seminar() { }

        public Seminar(int year, string location, DateTime begin, DateTime end)
        {
            Year = year;
            Location = location;
            Begin = begin;
            End = end;
        }

        #region Mapped Fields
        public virtual int Year { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string Location { get; set; }
        [Before("End")]
        public virtual DateTime Begin { get; set; }
        public virtual DateTime End { get; set; }

        // optional fields
        [StringLength(20)]
        public virtual string RegistrationPassword { get; set; }
        public virtual int? RegistrationId { get; set; }
        #endregion
    }

    public class SeminarMap : ClassMap<Seminar>
    {
        public SeminarMap()
        {
            Id(x => x.Id);

            Map(x => x.Year);
            Map(x => x.Location);
            Map(x => x.Begin).Column("`Begin`");
            Map(x => x.End).Column("`End`");
            Map(x => x.RegistrationPassword);
            Map(x => x.RegistrationId);
        }
    }
}
