using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class FirmType : DomainObject
    {
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
    }

    public class FirmTypeMap : ClassMap<FirmType>
    {
        public FirmTypeMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.IsActive);
        }
    }
}
