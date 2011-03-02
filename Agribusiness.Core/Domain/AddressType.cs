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
    }

    public class AddressTypeMap : ClassMap<AddressType>
    {
        public AddressTypeMap()
        {
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
