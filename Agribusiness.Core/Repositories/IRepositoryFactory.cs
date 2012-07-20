using System;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Agribusiness.Core.Repositories
{
    public interface IRepositoryFactory
    {
        IRepository<Address> AddressRepository { get; set; }
        IRepositoryWithTypedId<AddressType, char> AddressTypeRepository { get; set; }
        IRepository<Application> ApplicationRepository { get; set; }
        IRepository<Attachment> AttachmentRepository { get; set; }
        IRepository<CaseStudy> CaseStudyRepository { get; set; }
        IRepository<Commodity> CommodityRepository { get; set; }
        IRepositoryWithTypedId<CommunicationOption, string> CommunicationOptionRepository { get; set; }
        IRepository<Contact> ContactRepository { get; set; }
        IRepositoryWithTypedId<ContactType, char> ContactTypeRepsitory { get; set; }
        IRepositoryWithTypedId<Country, string> CountryRepository { get; set; }
        IRepository<EmailQueue> EmailQueueRepository { get; set; }
        IRepository<Firm> FirmRepository { get; set; }
        IRepository<FirmType> FirmTypeRepository { get; set; }
        IRepository<InformationRequest> InformationRequestRepository { get; set; }
        IRepository<InformationRequestNote> InformationRequestNoteRepository { get; set; }
        IRepository<Invitation> InvitationRepository { get; set; }
        IRepository<MailingList> MailingListRepository { get; set; }
        IRepository<NotificationTracking> NotificationTrackingRepository { get; set; }
        IRepository<Person> Person { get; set; }
        IRepository<RoomType> RoomTypeRepository { get; set; }
        IRepository<Seminar> SeminarRepository { get; set; }
        IRepository<SeminarPerson> SeminarPersonRepository { get; set; }
        IRepositoryWithTypedId<SeminarRole, string> SeminarRoleRepository { get; set; }
        IRepository<Session> SessionRepository { get; set; }
        IRepositoryWithTypedId<SessionType, string> SessionTypeRepository { get; set; }
        IRepositoryWithTypedId<Site, string> SiteRepository { get; set; }
        IRepository<Template> TemplateRepository { get; set; }
        IRepositoryWithTypedId<User, Guid> UserRepository { get; set; }
    }

    public class RepositoryFactory : IRepositoryFactory
    {
        public IRepository<Address> AddressRepository { get; set; }
        public IRepositoryWithTypedId<AddressType, char> AddressTypeRepository { get; set; }
        public IRepository<Application> ApplicationRepository { get; set; }
        public IRepository<Attachment> AttachmentRepository { get; set; }
        public IRepository<CaseStudy> CaseStudyRepository { get; set; }
        public IRepository<Commodity> CommodityRepository { get; set; }
        public IRepositoryWithTypedId<CommunicationOption, string> CommunicationOptionRepository { get; set; }
        public IRepository<Contact> ContactRepository { get; set; }
        public IRepositoryWithTypedId<ContactType, char> ContactTypeRepsitory { get; set; }
        public IRepositoryWithTypedId<Country, string> CountryRepository { get; set; }
        public IRepository<EmailQueue> EmailQueueRepository { get; set; }
        public IRepository<Firm> FirmRepository { get; set; }
        public IRepository<FirmType> FirmTypeRepository { get; set; }
        public IRepository<InformationRequest> InformationRequestRepository { get; set; }
        public IRepository<InformationRequestNote> InformationRequestNoteRepository { get; set; }
        public IRepository<Invitation> InvitationRepository { get; set; }
        public IRepository<MailingList> MailingListRepository { get; set; }
        public IRepository<NotificationTracking> NotificationTrackingRepository { get; set; }
        public IRepository<Person> Person { get; set; }
        public IRepository<RoomType> RoomTypeRepository { get; set; }
        public IRepository<Seminar> SeminarRepository { get; set; }
        public IRepository<SeminarPerson> SeminarPersonRepository { get; set; }
        public IRepositoryWithTypedId<SeminarRole, string> SeminarRoleRepository { get; set; }
        public IRepository<Session> SessionRepository { get; set; }
        public IRepositoryWithTypedId<SessionType, string> SessionTypeRepository { get; set; }
        public IRepositoryWithTypedId<Site, string> SiteRepository { get; set; }
        public IRepository<Template> TemplateRepository { get; set; }
        public IRepositoryWithTypedId<User, Guid> UserRepository { get; set; }
    }
}
