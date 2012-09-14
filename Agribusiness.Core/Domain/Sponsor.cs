using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Sponsor : DomainObject
    {
        public Sponsor()
        {
            IsActive = true;
            Order = 0;
        }

        [StringLength(100)]
        [Required]
        public virtual string Name { get; set; }
        public virtual byte[] Logo { get; set; }
        [StringLength(50)]
        public virtual string LogoContentType { get; set; }
        [Url]
        public virtual string Url { get; set; }
        public virtual SponsorLevel Level { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int Order { get; set; }
        public virtual Site Site { get; set; }
    }

    public class SponsorMap : ClassMap<Sponsor>
    {
        public SponsorMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Logo).CustomType("BinaryBlob");
            Map(x => x.LogoContentType);
            Map(x => x.Url);
            Map(x => x.Level).CustomType<NHibernate.Type.EnumStringType<SponsorLevel>>().Not.Nullable();
            Map(x => x.Description);
            Map(x => x.IsActive);
            Map(x => x.Order).Column("`Order`");
            References(x => x.Site);
        }
    }

    public enum SponsorLevel
    {
        Bronze = 1, Silver, Gold
    }
}
