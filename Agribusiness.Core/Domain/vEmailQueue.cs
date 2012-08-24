using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class VEmailQueue : DomainObject
    {
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual string FromAddress { get; set; }
        public virtual string Email { get; set; }

        public virtual Person Person { get; set; }
    }

    public class VEmailQueueMap : ClassMap<VEmailQueue>
    {
        public VEmailQueueMap()
        {
            ReadOnly();
            Table("vEmailQueue");

            Id(x => x.Id);

            Map(x => x.Subject);
            Map(x => x.Body);
            Map(x => x.FromAddress);
            Map(x => x.Email);

            References(x => x.Person).Fetch.Join();
        }
    }
}
