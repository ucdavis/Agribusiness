using System.Collections.Generic;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class Attachment : DomainObject
    {
        public Attachment()
        {
            EmailQueues = new List<EmailQueue>();
        }

        public virtual byte[] Contents { get; set; }
        public virtual string FileName { get; set; }
        public virtual string ContentType { get; set; }

        public virtual IList<EmailQueue> EmailQueues { get; set; }
    }

    public class AttachmentMap : ClassMap<Attachment>
    {
        public AttachmentMap()
        {
            Id(x => x.Id);

            Map(x => x.Contents).CustomType("BinaryBlob");
            Map(x => x.FileName);
            Map(x => x.ContentType);

            HasManyToMany(x => x.EmailQueues).ParentKeyColumn("AttachmentId").ChildKeyColumn("EmailQueueId").Table("EmailQueueXAttachments").Cascade.None();
        }
    }
}
