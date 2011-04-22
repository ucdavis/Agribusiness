using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Commodity : DomainObject
    {
        [Required]
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
    }

    public class CommodityMap : ClassMap<Commodity>
    {
        public CommodityMap()
        {
            Table("Commodities");
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.IsActive);
        }
    }
}
