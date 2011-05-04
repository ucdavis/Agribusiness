using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Country : DomainObjectWithTypedId<string>
    {
        public virtual string Name { get; set; }
    }

    public class CountryMap : ClassMap<Country>
    {
        public CountryMap()
        {
            ReadOnly();

            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
