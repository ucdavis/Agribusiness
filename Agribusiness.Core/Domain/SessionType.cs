using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class SessionType : DomainObjectWithTypedId<string>
    {
        [Required]
        [StringLength(50)]
        public virtual string Name { get; set; }
    }

    public class SessionTypeMap : ClassMap<SessionType>
    {
        public SessionTypeMap()
        {
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
