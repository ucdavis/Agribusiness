using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class User : DomainObjectWithTypedId<Guid>
    {
        public virtual string UserName { get; set; }
        public virtual string LoweredUserName { get; set; }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            ReadOnly();
            Table("aspnet_Users");

            Id(x => x.Id).Column("UserId");

            Map(x => x.UserName);
            Map(x => x.LoweredUserName);
        }
    }
}
