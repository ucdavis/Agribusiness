using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Template : DomainObject
    {
        public Template()
        {
            IsActive = true;
        }

        [Required]
        public virtual string BodyText { get; set; }
        public virtual bool IsActive { get; set; }
        [Required]
        public virtual NotificationType NotificationType { get; set; }
    }

    public class TemplateMap : ClassMap<Template>
    {
        public TemplateMap()
        {
            Id(x => x.Id);

            Map(x => x.BodyText);
            Map(x => x.IsActive);
            References(x => x.NotificationType);
        }
    }
}
