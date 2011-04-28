using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers;
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
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepositoryWithTypedId<User, Guid> _userRepository;
        private readonly IFirmService _firmService;

        public PersonService(IRepository<Firm> firmRepository, IRepository<Person> personRepository, IRepository<SeminarPerson> seminarPersonRepository, IRepository<Seminar> seminarRepository, IRepositoryWithTypedId<User, Guid> userRepository, IFirmService firmService)
        {
            _firmRepository = firmRepository;
            _personRepository = personRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _seminarRepository = seminarRepository;
            _userRepository = userRepository;
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

            displayPerson.Firm = reg.Firm;
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

        /// <summary>
        /// Load's a person object from a user's login id
        /// 
        /// Should only be used on actions where someone is participating in seminar (not applicants)
        /// </summary>
        /// <param name="loginId">user's login id</param>
        /// <returns>Null, means user not found or no person object</returns>
        public Person LoadPerson(string loginId)
        {
            // get the user's seminar person id
            var user = _userRepository.Queryable.Where(a => a.LoweredUserName == loginId.ToLower()).SingleOrDefault();
            if (user == null) return null;

            return user.Person;
        }

        /// <summary>
        /// Determines if a person has access to a seminar's information
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="seminarId">Seminar Id</param>
        /// <returns>True, has access</returns>
        public bool HasAccess(string loginId, int seminarId)
        {
            var person = LoadPerson(loginId);
            var seminar = _seminarRepository.GetNullableById(seminarId);

            return HasAccess(person, seminar);
        }

        /// <summary>
        /// Determines if a person has access to a seminar's information
        /// </summary>
        /// <returns>True, has access</returns>
        public bool HasAccess(string loginId, Seminar seminar)
        {
            var person = LoadPerson(loginId);

            return HasAccess(person, seminar);
        }

        /// <summary>
        /// Determines if a person has access to a seminar's information
        /// </summary>
        /// <param name="person"></param>
        /// <param name="seminar"></param>
        /// <returns>True, has access</returns>
        public bool HasAccess(Person person, Seminar seminar)
        {
            Check.Require(person != null, "person is required.");
            Check.Require(seminar != null, "seminar is required.");

            return person.SeminarPeople.Select(a => a.Seminar).Contains(seminar) && seminar.ReleaseToAttendees;
        }

        #region Helper Functions
        private IEnumerable<DisplayPerson> GetDisplayPeeps(IEnumerable<Person> people)
        {
            var displayPeople = new List<DisplayPerson>();
            
            foreach (var person in people)
            {
                var reg = person.GetLatestRegistration();
                if (reg != null)
                {
                    // only give back a firm if it's not null
                    var firm = !reg.Firm.Review ? reg.Firm : null;
                    displayPeople.Add(new DisplayPerson() { Firm = firm, Person = person, Title = reg.Title, Invite = reg.Invite, Registered = reg.Paid});
                }
                else
                {
                    displayPeople.Add(new DisplayPerson() { Person = person });
                }
            }

            return displayPeople;
        }
        #endregion
    }
}