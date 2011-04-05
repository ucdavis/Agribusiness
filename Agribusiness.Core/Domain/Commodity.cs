using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Commodity : DomainObject
    {
        public virtual string Name { get; set; }
    }

    public class CommodityMap : ClassMap<Commodity>
    {
        public CommodityMap()
        {
            Table("Commodities");
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
