using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Services
{
    public interface IEventService
    {
        void Invite(Person person, string siteId);
        void Apply(Person person, Application application, string siteId);
        void Accepted(Person person, string siteId);
        void Denied(Person person, string siteId);
        void Paid(Person person, string siteId);
        void HotelUpdate(Person person, string siteId);
        void PhotoUpdate(Person person, string siteId);
        void BioUpdate(Person person, string siteId);
    }
}