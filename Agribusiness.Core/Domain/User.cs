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
        public User()
        {
            People = new List<Person>();
        }

        public virtual string UserName { get; set; }
        public virtual string LoweredUserName { get; set; }
        public virtual Guid ApplicationId { get; set; }
        
        public virtual IList<Person> People { get; set; }
        public virtual IList<Application> Applications { get; set; }
        public virtual IList<Membership> Memberships { get; set; }

        /// <summary>
        /// Returns the theoretical single person
        /// </summary>
        public virtual Person Person { get { return People.FirstOrDefault(); } }

        public virtual Membership Membership { get { return Memberships.FirstOrDefault(x => x.ApplicationId == this.ApplicationId); } }

        /// <summary>
        /// Returns email if membership is available otherwise, give back the lowered username
        /// </summary>
        public virtual string Email
        {
            get { 
                var membership = Memberships.FirstOrDefault(x => x.ApplicationId == this.ApplicationId);

                if (membership != null)
                {
                    return membership.Email;
                }

                return LoweredUserName;
            }
        }

        public virtual void SetUserName(string userName)
        {
            userName = userName.Replace(" ", string.Empty);

            UserName = userName;
            LoweredUserName = userName.ToLower();
        }
    }

    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("aspnet_Users");

            Id(x => x.Id).Column("UserId");

            Map(x => x.UserName);
            Map(x => x.LoweredUserName);
            Map(x => x.ApplicationId);

            HasMany(x => x.People).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Applications).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Memberships).KeyColumn("UserId").Inverse().Cascade.AllDeleteOrphan();
        }
    }

    public class Membership : DomainObjectWithTypedId<Guid>
    {
        public virtual string Email { get; set; }
        public virtual string LoweredEmail { get; set; }
        public virtual Guid ApplicationId { get; set; }
        public virtual User User { get; set; }

        public virtual void SetEmail(string email)
        {
            Email = email;
            LoweredEmail = email.ToLower();
        }
    }

    public class MembershipMap : ClassMap<Membership>
    {
        public MembershipMap()
        {
            Table("aspnet_Membership");

            Id(x => x.Id).Column("UserId");

            Map(x => x.Email);
            Map(x => x.LoweredEmail);
            Map(x => x.ApplicationId);
            References(x => x.User);
        }
    }
}
