using System;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Firm : DomainObject
    {
        public Firm()
        {
            FirmCode = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor for creating/updating
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="firmCode">If updating an existing firm, pass this parameter</param>
        public Firm(string name, string description, Guid? firmCode = null)
        {
            Name = name;
            Description = description;
            FirmCode = firmCode ?? Guid.NewGuid();

            Review = true;
        }

        public virtual Guid FirmCode { get; set; }
        [Required]
        [StringLength(200)]
        public virtual string Name { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public virtual string Description { get; set; }

        public virtual bool Review { get; set; }
        [Display(Name="Web Address")]
        [StringLength(200)]
        [DataType(DataType.Url)]
        public virtual string WebAddress { get; set; }
    }

    public class FirmMap : ClassMap<Firm>
    {
        public FirmMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Description);
            Map(x => x.FirmCode);
            Map(x => x.Review);
            Map(x => x.WebAddress);
        }
    }
}
