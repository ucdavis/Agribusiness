using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class SeminarRole : DomainObjectWithTypedId<string>
    {
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual decimal? Discount { get; set; }
        public virtual string Description { get; set; }
    }

    public class SeminarRoleMap : ClassMap<SeminarRole>
    {
        public SeminarRoleMap()
        {
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Discount);
            Map(x => x.Description);
        }
    }
}
