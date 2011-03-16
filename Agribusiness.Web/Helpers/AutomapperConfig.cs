using Agribusiness.Core.Domain;
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
            CreateMap<Seminar, Seminar>().ForMember(x=>x.Id, x=>x.Ignore()).ForMember(x=>x.Sessions,x=>x.Ignore()).ForMember(x=>x.SeminarPeople, x=>x.Ignore());
            CreateMap<Session, Session>().ForMember(x => x.Id, x => x.Ignore()).ForMember(x=>x.Seminar, x=>x.Ignore());
        }
        
    }
}