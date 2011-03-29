using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Seminar : DomainObject
    {
        public Seminar()
        {
            SetDefaults();
        }

        public Seminar(int year, string location, DateTime begin, DateTime end)
        {
            Year = year;
            Location = location;
            Begin = begin;
            End = end;
        }

        private void SetDefaults()
        {
            Year = DateTime.Now.Year;

            Begin = DateTime.Now;
            End = DateTime.Now;

            Sessions = new List<Session>();
            SeminarPeople = new List<SeminarPerson>();
            CaseStudies = new List<CaseStudy>();
        }

        #region Mapped Fields
        public virtual int Year { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string Location { get; set; }
        [Before("End")]
        public virtual DateTime Begin { get; set; }
        public virtual DateTime End { get; set; }
        [Before("End")]
        public virtual DateTime? RegistrationDeadline { get; set; }

        // optional fields
        [StringLength(20)]
        public virtual string RegistrationPassword { get; set; }
        public virtual int? RegistrationId { get; set; }

        public virtual IList<Session> Sessions { get; set; }
        public virtual IList<SeminarPerson> SeminarPeople { get; set; }
        public virtual IList<CaseStudy> CaseStudies { get; set; }
        #endregion

        /// <summary>
        /// Short Date Time string for Registration Deadline, if not specified returns "n/a"
        /// </summary>
        public virtual string RegistrationDeadlineString { get { return RegistrationDeadline.HasValue ? RegistrationDeadline.Value.ToShortDateString() : "n/a"; } }
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
            Map(x => x.RegistrationDeadline);
            Map(x => x.RegistrationPassword);
            Map(x => x.RegistrationId);

            HasMany(x => x.Sessions).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.SeminarPeople).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.CaseStudies).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
