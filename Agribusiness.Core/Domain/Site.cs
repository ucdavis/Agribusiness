using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Site : DomainObjectWithTypedId<string>
    {
        public Site()
        {
            IsActive = true;
            Seminars = new List<Seminar>();
            People = new List<Person>();
        }

        [StringLength(100)]
        [Required]
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
        [StringLength(50)]
        public virtual string EventType { get; set; }
        /// <summary>
        /// Collect extended information on More Information Page
        /// </summary>
        public virtual bool CollectExtended { get; set; }
        public virtual string Subdomain { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }
        [Display(Name = "Welcome Text")]
        [DataType(DataType.MultilineText)]
        public virtual string Welcome { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string Background { get; set; }
        public virtual Person BackgroundPerson { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string Venue { get; set; }
        
        public virtual string VenueEmbeddedMap { get; set; }

        public virtual byte[] Logo { get; set; }
        public virtual string LogoContentType { get; set; }
        public virtual byte[] SplashImage { get; set; }
        public virtual string SplashContentType { get; set; }

        public virtual IList<Seminar> Seminars { get; set; }
        public virtual IList<Person> People { get; set; }
        public virtual IList<Sponsor> Sponsors { get; set; }
    }

    public class SiteMap : ClassMap<Site>
    {
        public SiteMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.EventType);
            Map(x => x.CollectExtended);
            Map(x => x.Subdomain);
            Map(x => x.IsActive);

            Map(x => x.Logo).CustomType("BinaryBlob");
            Map(x => x.LogoContentType);
            Map(x => x.SplashImage).CustomType("BinaryBlob");
            Map(x => x.SplashContentType);

            Map(x => x.Description).Length(int.MaxValue);
            Map(x => x.Welcome).Length(int.MaxValue);
            Map(x => x.Background).Length(int.MaxValue);
            References(x => x.BackgroundPerson).Column("BackgroundPersonId");
            Map(x => x.Venue).Length(int.MaxValue);
            Map(x => x.VenueEmbeddedMap).Length(int.MaxValue);

            HasMany(x => x.Seminars);
            HasManyToMany(x => x.People)
                .ParentKeyColumn("SiteId")
                .ChildKeyColumn("PersonId")
                .Table("PeopleXSites")
                .Cascade.SaveUpdate();
            HasMany(x => x.Sponsors);
        }
    }
}
