using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class AddressType : DomainObjectWithTypedId<char>
    {
        [StringLength(50)]
        [Required]
        public virtual string Name { get; set; }

        [StringLength(200)]
        public virtual string Description { get; set; }

        public virtual bool Required { get; set; }
    }

    public class AddressTypeMap : ClassMap<AddressType>
    {
        public AddressTypeMap()
        {
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Required);
            Map(x => x.Description);
        }
    }
}
