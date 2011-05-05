using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class CommunicationOption : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool RequiresAssistant { get; set; }
    }

    public class CommunicationOptionMap : ClassMap<CommunicationOption>
    {
        public CommunicationOptionMap()
        {
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.RequiresAssistant);
        }
    }
}
