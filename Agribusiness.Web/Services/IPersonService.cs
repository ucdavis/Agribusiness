using System.Collections.Generic;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;

namespace Agribusiness.Web.Services
{
    public interface IPersonService
    {
        DisplayPerson GetDisplayPerson(Person person, Seminar seminar = null);
        IEnumerable<DisplayPerson> GetAllDisplayPeople();
        IEnumerable<DisplayPerson> GetDisplayPeopleForSeminar(int id);
        IEnumerable<DisplayPerson> GetDisplayPeopleNotInSeminar(int id);
        IEnumerable<DisplayPerson> ConvertToDisplayPeople(IEnumerable<Person> people);
            
        Person LoadPerson(string loginId);
        bool HasAccess(string loginId, int seminarId, bool paidResources = true);
        bool HasAccess(string loginId, Seminar seminar, bool paidResources = true);
        bool HasAccess(Person person, Seminar seminar, bool paidResources = true);

        List<KeyValuePair<Person, string>> ResetPasswords(List<Person> people);

        Person CreateSeminarPerson(Application application, ModelStateDictionary modelState);
        void UpdatePerson(Person person, Application application);
    }
}
