using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;
using System.Linq;

namespace Agribusiness.Core.Domain
{
    public class CaseStudy : DomainObject
    {
        #region Constructors
        public CaseStudy() { }

        public CaseStudy(string name, byte[] file, Seminar seminar, Session session)
        {
            Name = name;
            File = file;
            Seminar = seminar;
            Session = session;

            SetDefaults();
        }

        private void SetDefaults()
        {
            CaseExecutives =  new List<SeminarPerson>();
            CaseAuthors = new List<SeminarPerson>();
        }

        #endregion

        #region Mapped Fields
        [Required]
        [StringLength(100)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }
        [Required]
        public virtual byte[] File { get; set; }
        
        public virtual Seminar Seminar { get; set; }
        public virtual Session Session { get; set; }

        public virtual IList<SeminarPerson> CaseExecutives { get; set; }
        public virtual IList<SeminarPerson> CaseAuthors { get; set; }
        #endregion

        #region Calculated Fields

        /// <summary>
        /// Comma seperated string of all case authors
        /// </summary>
        public virtual string Authors { get { return string.Join(", ", CaseAuthors.Select(a=>a.Person.FullName)); } }
        /// <summary>
        /// Comma seperated string of all case executives
        /// </summary>
        public virtual string Executives { get { return string.Join(", ", CaseExecutives.Select(a=>a.Person.FullName)); } }
        #endregion
    }

    public class CaseStudyMap : ClassMap<CaseStudy>
    {
        public CaseStudyMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.File).Column("`File`").LazyLoad().CustomType("BinaryBlob");

            References(x => x.Seminar);
            References(x => x.Session);

            HasManyToMany(x => x.CaseExecutives)
                .ParentKeyColumn("CaseStudyId")
                .ChildKeyColumn("SeminarPersonId")
                .Table("CaseStudyExecutives")
                .Cascade.SaveUpdate();

            HasManyToMany(x => x.CaseAuthors)
                .ParentKeyColumn("CaseStudyId")
                .ChildKeyColumn("SeminarPersonId")
                .Table("CaseStudyAuthors")
                .Cascade.SaveUpdate();            
        }
    }
}
