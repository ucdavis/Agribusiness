using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

            ReleaseToAttendees = false;
            ReleaseCaseStudyList = false;
            ReleaseSchedule = false;

            RequireInvitation = true;
            RequireApproval = true;

            Sessions = new List<Session>();
            SeminarPeople = new List<SeminarPerson>();
            CaseStudies = new List<CaseStudy>();
            Applications = new List<Application>();
            Templates = new List<Template>();
        }

        #region Mapped Fields
        public virtual int Year { get; set; }
        [Required]
        [StringLength(100)]
        public virtual string Location { get; set; }
        [StringLength(200)]
        [DataType(DataType.Url)]
        public virtual string LocationLink { get; set; }
        [Before("End")]
        public virtual DateTime Begin { get; set; }
        public virtual DateTime End { get; set; }
        [Before("Begin")]
        public virtual DateTime? RegistrationBegin { get; set; }
        [Before("Begin")]
        public virtual DateTime? RegistrationDeadline { get; set; }
        
        public virtual DateTime? AcceptanceDate { get; set; }

        public virtual bool ReleaseToAttendees { get; set; }
        public virtual decimal? Cost { get; set; }

        // optional fields
        [StringLength(20)]
        public virtual string RegistrationPassword { get; set; }
        public virtual int? RegistrationId { get; set; }

        public virtual bool ReleaseCaseStudyList { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string CaseStudyText { get; set; }

        public virtual bool ReleaseSchedule { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string ProgramInformation { get; set; }

        public virtual DateTime? PaymentDeadline { get; set; }

        public virtual bool RequireInvitation { get; set; }
        public virtual bool RequireApproval { get; set; }

        public virtual Site Site { get; set; }

        public virtual IList<Session> Sessions { get; set; }
        public virtual IList<SeminarPerson> SeminarPeople { get; set; }
        public virtual IList<CaseStudy> CaseStudies { get; set; }
        public virtual IList<MailingList> MailingLists { get; set; }
        public virtual IList<Invitation> Invitations { get; set; }
        public virtual IList<File> Files { get; set; }
        public virtual IList<Application> Applications { get; set; }
        public virtual IList<Template> Templates { get; set; }
        #endregion

        /// <summary>
        /// Short Date Time string for Registration Deadline, if not specified returns "n/a"
        /// </summary>
        public virtual string RegistrationDeadlineString { get { return RegistrationDeadline.HasValue ? RegistrationDeadline.Value.ToShortDateString() : "n/a"; } }

        public virtual string RegistrationDates
        {
            get
            {
                if (RegistrationBegin.HasValue && RegistrationDeadline.HasValue)
                {
                    return string.Format("{0} - {1}", RegistrationBegin.Value.ToShortDateString(), RegistrationDeadline.Value.ToShortDateString());
                }

                if (RegistrationBegin.HasValue)
                {
                    return string.Format("Beginning {0}", RegistrationBegin.Value.ToShortDateString());
                }

                if (RegistrationDeadline.HasValue)
                {
                    return string.Format("Through {0}", RegistrationDeadline.Value.ToShortDateString());
                }

                return "n/a";
            }
        }

        public virtual void AddTemplate(Template template)
        {
            var existing = Templates.FirstOrDefault(a => a.NotificationType == template.NotificationType);

            if (existing != null)
            {
                existing.IsActive = false;
            }

            Templates.Add(template);
        }

        /// <summary>
        /// Whether this seminar is taking applications, based on registration deadlines
        /// </summary>
        public virtual bool OpenForRegistration
        {
            get
            {
                // just a begin date, no end
                if (RegistrationBegin.HasValue && !RegistrationDeadline.HasValue)
                {
                    return RegistrationBegin.Value.Date <= DateTime.Now.Date;
                }
                
                if (!RegistrationBegin.HasValue && RegistrationDeadline.HasValue)
                {
                    return RegistrationDeadline.Value.Date >= DateTime.Now.Date;
                }
                
                if (RegistrationBegin.HasValue && RegistrationDeadline.HasValue)
                {
                    return RegistrationBegin.Value.Date <= DateTime.Now.Date && RegistrationDeadline.Value.Date >= DateTime.Now.Date;
                }

                // no dates specified
                return false;
            }

        }
    }

    public class SeminarMap : ClassMap<Seminar>
    {
        public SeminarMap()
        {
            Id(x => x.Id);

            Map(x => x.Year);
            Map(x => x.Location);
            Map(x => x.LocationLink);
            Map(x => x.Begin).Column("`Begin`");
            Map(x => x.End).Column("`End`");
            Map(x => x.RegistrationBegin);
            Map(x => x.RegistrationDeadline);
            Map(x => x.RegistrationPassword);
            Map(x => x.AcceptanceDate);
            Map(x => x.RegistrationId);
            Map(x => x.ReleaseToAttendees);
            Map(x => x.Cost);
            References(x => x.Site);

            Map(x => x.ReleaseCaseStudyList);
            Map(x => x.CaseStudyText);

            Map(x => x.ReleaseSchedule);
            Map(x => x.ProgramInformation);
            Map(x => x.PaymentDeadline);

            Map(x => x.RequireApproval);
            Map(x => x.RequireInvitation);

            HasMany(x => x.Sessions).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.SeminarPeople).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.CaseStudies).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.MailingLists).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Invitations).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Files).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Applications).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Templates).Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
