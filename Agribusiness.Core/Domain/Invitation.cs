using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Invitation : DomainObject
    {
        public Invitation()
        {
            
        }

        public Invitation(Person person)
        {
            Person = person;
        }

        public virtual Person Person { get; set; }
        public virtual Seminar Seminar { get; set; }

        [StringLength(200)]
        public virtual string Title { get; set; }
        [StringLength(200)]
        [DisplayName("Firm Name")]
        public virtual string FirmName { get; set; }

    }

    public class InvitationMap : ClassMap<Invitation>
    {
        public InvitationMap()
        {
            Id(x => x.Id);

            References(x => x.Person);
            References(x => x.Seminar);
            Map(x => x.Title);
            Map(x => x.FirmName);
        }
    }
}
