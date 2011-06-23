using System;
using System.ComponentModel.DataAnnotations;
using FluentNHibernate.Mapping;
using UCDArch.Core.DomainModel;

namespace Agribusiness.Core.Domain
{
    public class InformationRequestNote : DomainObject
    {
        public InformationRequestNote()
        {
            SetDefaults();
        }

        public InformationRequestNote(InformationRequest informationRequest, string notes, string userName)
        {
            InformationRequest = informationRequest;
            Notes = notes;
            NoteBy = userName;

            SetDefaults();
        }

        private void SetDefaults()
        {
            DateTimeNote = DateTime.Now;
        }
        
        [Required]
        public virtual InformationRequest InformationRequest { get; set; }
        [Required]
        public virtual string Notes { get; set; }
        public virtual DateTime DateTimeNote { get; set; }
        [StringLength(15)]
        [Required]
        public virtual string NoteBy { get; set; }
    }

    public class InformationRequestNoteMap : ClassMap<InformationRequestNote>
    {
        public InformationRequestNoteMap()
        {
            Id(x => x.Id);

            References(x => x.InformationRequest);
            Map(x => x.Notes);
            Map(x => x.DateTimeNote);
            Map(x => x.NoteBy);
        }
    }
}
