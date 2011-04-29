using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class ContactType : DomainObjectWithTypedId<char>
    {
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }

        public virtual bool Required { get; set; }
    }

    public class ContactTypeMap : ClassMap<ContactType>
    {
        public ContactTypeMap()
        {
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Required);
        }
    }
}
