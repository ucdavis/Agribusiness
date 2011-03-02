using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Session : DomainObject
    {
        #region Constructors
        public Session() { }

        public Session(string name, SessionType sessionType, Seminar seminar)
        {
            Name = name;
            SessionType = sessionType;
            Seminar = seminar;
        }
        #endregion

        #region Mapped Fields
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }
        [StringLength(50)]
        public virtual string Location { get; set; }
        public virtual DateTime? Begin { get; set; }
        public virtual DateTime? End { get; set; }
        [Required]
        public virtual SessionType SessionType { get; set; }
        [Required]
        public virtual Seminar Seminar { get; set; }

        public virtual IList<SeminarPerson> SeminarPeople { get; set; }
        #endregion
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

            HasManyToMany(x => x.SeminarPeople)
                .ParentKeyColumn("SessionId")
                .ChildKeyColumn("SeminarPersonId")
                .Table("SeminarPeopleXSessions")
                .Cascade.SaveUpdate();
        }
    }
}
