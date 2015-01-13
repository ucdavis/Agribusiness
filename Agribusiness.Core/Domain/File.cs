using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class File : DomainObject
    {
        public File()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            Public = false;
            MySeminar = false;
            ProgramOverview = false;
        }

        public virtual string Name { get; set; }
        public virtual byte[] Contents { get; set; }
        public virtual string ContentType { get; set; }
        public virtual string FileName { get; set; }
        public virtual Seminar Seminar { get; set; }

        public virtual bool Public { get; set; }
        public virtual bool MySeminar { get; set; }
        public virtual bool ProgramOverview { get; set; }
        public virtual bool Sponsors { get; set; }
        public virtual bool Home { get; set; }
        public virtual bool Venue { get; set; }
    }

    public class FileMap : ClassMap<File>
    {
        public FileMap()
        {
            Id(x => x.Id);

            Map(x => x.Name);
            Map(x => x.Contents).CustomType("BinaryBlob");
            Map(x => x.ContentType);
            Map(x => x.FileName);
            References(x => x.Seminar);

            Map(x => x.Public).Column("`Public`");
            Map(x => x.MySeminar);
            Map(x => x.ProgramOverview);
            Map(x => x.Sponsors);
            Map(x => x.Home);
            Map(x => x.Venue);
        }
    }
}
