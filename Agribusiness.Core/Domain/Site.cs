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
        }

        [StringLength(100)]
        [Required]
        public virtual string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual byte[] Logo { get; set; }
        public virtual string LogoContentType { get; set; }
        public virtual byte[] SplashImage { get; set; }
        public virtual string SplashContentType { get; set; }
    }

    public class SiteMap : ClassMap<Site>
    {
        public SiteMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.IsActive);

            Map(x => x.Logo).CustomType("BinaryBlob");
            Map(x => x.LogoContentType);
            Map(x => x.SplashImage).CustomType("BinaryBlob");
            Map(x => x.SplashContentType);
        }
    }
}
