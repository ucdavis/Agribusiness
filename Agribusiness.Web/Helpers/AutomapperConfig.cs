﻿using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using AutoMapper;

namespace Agribusiness.Web.Helpers
{
    public class AutomapperConfig
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg=>cfg.AddProfile<ViewModelProfile>());
        }
    }

    public class ViewModelProfile : Profile
    {
        protected override void  Configure()
        {
            CreateMap<Seminar, Seminar>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Sessions, x => x.Ignore())
                .ForMember(x => x.SeminarPeople, x => x.Ignore())
                .ForMember(x => x.CaseStudies, x => x.Ignore());
            
            CreateMap<Session, Session>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x=>x.Seminar, x=>x.Ignore());

            CreateMap<Commodity, Commodity>()
                .ForMember(x => x.Id, x => x.Ignore());

            CreateMap<PersonEditModel, Person>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.FirstName, x => x.MapFrom(a => a.Person.FirstName))
                .ForMember(x => x.MI, x => x.MapFrom(a => a.Person.MI))
                .ForMember(x => x.LastName, x => x.MapFrom(a => a.Person.LastName))
                .ForMember(x => x.Salutation, x => x.MapFrom(a => a.Person.Salutation))
                .ForMember(x => x.BadgeName, x => x.MapFrom(a => a.Person.BadgeName))
                .ForMember(x => x.Phone, x => x.MapFrom(a => a.Person.Phone))
                .ForMember(x => x.CellPhone, x => x.MapFrom(a => a.Person.CellPhone))
                .ForMember(x => x.Fax, x => x.MapFrom(a => a.Person.Fax))
                .ForMember(x => x.Addresses, x => x.Ignore())
                .ForMember(x => x.Contacts, x => x.Ignore());

            CreateMap<Address, Address>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.AddressType, x => x.Ignore())
                .ForMember(x => x.Person, x => x.Ignore());

            CreateMap<Contact, Contact>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.ContactType, x => x.Ignore())
                .ForMember(x => x.Person, x => x.Ignore());

            CreateMap<Firm, Firm>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Review, x => x.Ignore());

            CreateMap<NotificationTracking, NotificationTracking>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Person, x => x.Ignore())
                .ForMember(x => x.Application, x => x.Ignore())
                .ForMember(x => x.EmailQueue, x => x.Ignore());

            CreateMap<EmailQueue, EmailQueue>()
                .ForMember(x => x.Id, x => x.Ignore())
                .ForMember(x => x.Person, x => x.Ignore());

            CreateMap<Template, Template>()
                .ForMember(x => x.Id, x => x.Ignore());
        }
        
    }
}