using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Firm> _firmRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;
        private readonly IFirmService _firmService;

        public PersonService(IRepository<Firm> firmRepository, IRepository<Person> personRepository, IRepository<SeminarPerson> seminarPersonRepository, IFirmService firmService)
        {
            _firmRepository = firmRepository;
            _personRepository = personRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _firmService = firmService;
        }

        /// <summary>
        /// Generates person with the associated latest revision of firm
        /// </summary>
        /// <param name="person"></param>
        /// <param name="firms">List of firms, there should only be one instance of each firm in this list</param>
        /// <returns></returns>
        public DisplayPerson GetDisplayPerson(Person person)
        {
            Check.Require(person != null, "person is required.");

            var displayPerson = new DisplayPerson() {Person = person};

            var reg = person.GetLatestRegistration();
            if (reg == null) return displayPerson;

            displayPerson.Firm = _firmService.GetFirm(reg.FirmCode);
            displayPerson.Title = reg.Title;
            return displayPerson;
        }

        /// <summary>
        /// Gets a display list of all people in the database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DisplayPerson> GetAllDisplayPeople()
        {
            var people = _personRepository.GetAll();
            return GetDisplayPeeps(people);
        }

        /// <summary>
        /// Gets a display list of people for a specific seminar
        /// </summary>
        /// <param name="id">Seminar Id</param>
        /// <returns></returns>
        public IEnumerable<DisplayPerson> GetDisplayPeopleForSeminar(int id)
        {
            var seminarPeeps = _seminarPersonRepository.Queryable.Where(a => a.Seminar.Id == id);
            var people = seminarPeeps.Select(a => a.Person);

            return GetDisplayPeeps(people);
        }

        private IEnumerable<DisplayPerson> GetDisplayPeeps(IEnumerable<Person> people)
        {
            var displayPeople = new List<DisplayPerson>();
            
            var firms = _firmService.GetAllFirms();

            foreach (var person in people)
            {
                var reg = person.GetLatestRegistration();
                if (reg != null)
                {
                    var firm = firms.Where(b => b.FirmCode == reg.FirmCode).FirstOrDefault();
                    displayPeople.Add(new DisplayPerson() { Firm = firm, Person = person, Title = reg.Title});
                }
                else
                {
                    displayPeople.Add(new DisplayPerson() { Person = person });
                }
            }

            return displayPeople;            
        }
        
    }
}