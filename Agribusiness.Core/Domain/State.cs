using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class State : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
    }

    public class StateMap : ClassMap<State>
    {
        public StateMap()
        {
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
