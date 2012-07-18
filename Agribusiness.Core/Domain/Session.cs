using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Session : DomainObject
    {
        #region Constructors
        public Session() { SetDefaults(); }

        public Session(string name, SessionType sessionType, Seminar seminar)
        {
            Name = name;
            SessionType = sessionType;
            Seminar = seminar;

            SetDefaults();
        }

        private void SetDefaults()
        {
            ShowPublic = true;

            SeminarPeople = new List<SeminarPerson>();
            CaseStudies = new List<CaseStudy>();
        }

        #endregion

        #region Mapped Fields
        [Required]
        public virtual string Name { get; set; }
        [StringLength(50)]
        public virtual string Location { get; set; }
        public virtual string Description { get; set; }
        [Before("End")]
        public virtual DateTime? Begin { get; set; }
        public virtual DateTime? End { get; set; }
        [Required]
        public virtual SessionType SessionType { get; set; }
        [Required]
        public virtual Seminar Seminar { get; set; }
        [Display(Name="Show Publicly")]
        public virtual bool ShowPublic { get; set; }

        public virtual IList<SeminarPerson> SeminarPeople { get; set; }
        public virtual IList<CaseStudy> CaseStudies { get; set; }
        /// <summary>
        /// List of people assigned as speaker/panelists/discussion group leaders
        /// </summary>
        public virtual IList<SeminarPerson> SessionPeople { get; set; }
        #endregion

        public virtual string BeginString { get { return Begin.HasValue ? Begin.Value.ToString("g") : "n/a"; } }
        public virtual string EndString { get { return End.HasValue ? End.Value.ToString("g") : "n/a"; } }

        public virtual int AttendeeCount {
            get { return SeminarPeople.Count; }
        }
    }

    public class SessionMap : ClassMap<Session>
    {
        public SessionMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Location);
            Map(x => x.Begin).Column("`Begin`");
            Map(x => x.End).Column("`End`");
            References(x => x.SessionType);
            References(x => x.Seminar);
            Map(x => x.ShowPublic);

            HasManyToMany(x => x.SeminarPeople)
                .ParentKeyColumn("SessionId")
                .ChildKeyColumn("SeminarPersonId")
                .Table("SeminarPeopleXSessions")
                .Cascade.SaveUpdate();

            HasMany(x => x.CaseStudies).Inverse().Cascade.AllDeleteOrphan();

            HasManyToMany(x => x.SessionPeople)
                .ParentKeyColumn("SessionId")
                .ChildKeyColumn("SeminarPersonId")
                .Table("SessionPeople")
                .Cascade.SaveUpdate();
        }
    }
}
