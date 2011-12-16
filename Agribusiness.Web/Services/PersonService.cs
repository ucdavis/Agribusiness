using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.Web.Controllers;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;
using UCDArch.Web.Helpers;

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
        private readonly IRepositoryWithTypedId<AddressType, char> _addressTypeRepository;
        private readonly IRepositoryWithTypedId<ContactType, char> _contactTypeRepository;
        private readonly IRepository<Commodity> _commodityRepository;
        private AccountMembershipService _membershipService;

        public PersonService(IRepository<Firm> firmRepository, IRepository<Person> personRepository, IRepository<SeminarPerson> seminarPersonRepository, IRepository<Seminar> seminarRepository, IRepositoryWithTypedId<User, Guid> userRepository, IFirmService firmService, IRepositoryWithTypedId<AddressType, char> addressTypeRepository, IRepositoryWithTypedId<ContactType, char> contactTypeRepository, IRepository<Commodity> commodityRepository )
        {
            _firmRepository = firmRepository;
            _personRepository = personRepository;
            _seminarPersonRepository = seminarPersonRepository;
            _seminarRepository = seminarRepository;
            _userRepository = userRepository;
            _firmService = firmService;
            _addressTypeRepository = addressTypeRepository;
            _contactTypeRepository = contactTypeRepository;
            _commodityRepository = commodityRepository;

            _membershipService = new AccountMembershipService();
        }

        /// <summary>
        /// Generates person with the associated latest revision of firm
        /// </summary>
        /// <param name="person"></param>
        /// <param name="firms">List of firms, there should only be one instance of each firm in this list</param>
        /// <returns></returns>
        public DisplayPerson GetDisplayPerson(Person person, Seminar seminar = null)
        {
            Check.Require(person != null, "person is required.");

            var displayPerson = new DisplayPerson() {Person = person};

            var reg = seminar == null ? person.GetLatestRegistration() : person.SeminarPeople.Where(a=>a.Seminar == seminar).FirstOrDefault();
            if (reg == null) return displayPerson;

            displayPerson.Seminar = reg.Seminar;
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
        /// Gets a display list of people who are not currently in a specific seminar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<DisplayPerson> GetDisplayPeopleNotInSeminar(int id)
        {
            // ids of people who we need to exclude
            var seminarPeeps = _seminarPersonRepository.Queryable.Where(a => a.Seminar.Id == id).Select(a=>a.Person.Id).ToList();

            // people who are not in the current seminar
            var people = _personRepository.Queryable.Where(a => !seminarPeeps.Contains(a.Id));


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
        public bool HasAccess(string loginId, int seminarId, bool paidResources = true)
        {
            var person = LoadPerson(loginId);
            var seminar = _seminarRepository.GetNullableById(seminarId);

            return HasAccess(person, seminar, paidResources);
        }

        /// <summary>
        /// Determines if a person has access to a seminar's information
        /// </summary>
        /// <returns>True, has access</returns>
        public bool HasAccess(string loginId, Seminar seminar, bool paidResources = true)
        {
            var person = LoadPerson(loginId);

            return HasAccess(person, seminar, paidResources);
        }

        /// <summary>
        /// Determines if a person has access to a seminar's information
        /// </summary>
        /// <param name="person"></param>
        /// <param name="seminar"></param>
        /// <returns>True, has access</returns>
        public bool HasAccess(Person person, Seminar seminar, bool paidResources = true)
        {
            Check.Require(person != null, "person is required.");
            Check.Require(seminar != null, "seminar is required.");

            if (paidResources)
            {
                return person.SeminarPeople.Any(a => a.Seminar == seminar && a.Paid) && seminar.ReleaseToAttendees;    
            }
            else
            {
                return person.SeminarPeople.Any(a => a.Seminar == seminar) && seminar.ReleaseToAttendees;
            }

            
        }

        public List<KeyValuePair<Person, string>> ResetPasswords(List<Person> people)
        {

            var result = new List<KeyValuePair<Person, string>>();

            foreach (var person in people)
            {
                var password = _membershipService.ResetPasswordNoEmail(person.User.UserName);
                result.Add(new KeyValuePair<Person, string>(person, password));
            }

            return result;

        }

        public Person CreateSeminarPerson(Application application, ModelStateDictionary modelState)
        {
            var person = SetPerson(application, application.User.Person);

            var firm = application.Firm ?? new Firm(application.FirmName, application.FirmDescription);

            var seminarPerson = new SeminarPerson()
            {
                Seminar = application.Seminar,
                Title = application.JobTitle,
                Firm = firm,
                Commodities = new List<Commodity>(application.Commodities),
                FirmType = application.FirmType,
                OtherFirmType = application.OtherFirmType
            };

            if (!string.IsNullOrWhiteSpace(application.OtherCommodity))
            {
                // transfer "other" commodities
                var others = application.OtherCommodity.Split(',');
                if (others.Count() > 0)
                {
                    foreach (var com in others)
                    {
                        var existing = _commodityRepository.Queryable.Where(a => a.Name == com).FirstOrDefault();

                        // check for an existing commodity
                        if (existing != null)
                        {
                            // assign that commodity if it exists
                            seminarPerson.Commodities.Add(existing);
                        }
                        else
                        {
                            // otherwise create a new one           
                            var newcom = new Commodity() { IsActive = false, Name = com };
                            seminarPerson.Commodities.Add(newcom);
                        }
                    }
                }    
            }
            

            person.AddSeminarPerson(seminarPerson);

            UpdateAddress(person, application);

            UpdateAssistant(person, application);

            person.TransferValidationMessagesTo(modelState);
            seminarPerson.TransferValidationMessagesTo(modelState);

            if (modelState.IsValid)
            {
                _firmRepository.EnsurePersistent(firm);
                _personRepository.EnsurePersistent(person);

                return person;
            }

            return null;
        }

        public void UpdatePerson(Person person, Application application)
        {
            // copy the primary information
            person = SetPerson(application, person);

            // fill in the address information
            UpdateAddress(person, application);

            // fill in the assistant information
            UpdateAssistant(person, application);

            // save
            _personRepository.EnsurePersistent(person);
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
                    displayPeople.Add(new DisplayPerson() { Firm = firm, Person = person, Title = reg.Title, Invite = reg.Invite, Registered = reg.Paid, Seminar = reg.Seminar});
                }
                else
                {
                    displayPeople.Add(new DisplayPerson() { Person = person});
                }
            }

            return displayPeople;
        }

        private Person SetPerson(Application application, Person person = null)
        {
            if (person == null)
            {
                person = new Person()
                {
                    LastName = application.LastName,
                    MI = application.MI,
                    FirstName = application.FirstName,
                    BadgeName = string.IsNullOrWhiteSpace(application.BadgeName) ? application.FirstName : application.BadgeName,
                    Phone = application.FirmPhone,
                    PhoneExt = application.FirmPhoneExt,
                    User = application.User,
                    OriginalPicture = application.Photo,
                    ContentType = application.ContentType,
                    CommunicationOption = application.CommunicationOption,
                    ContactInformationRelease = application.ContactInformationRelease
                };    
            }
            else
            {
                person.LastName = application.LastName;
                person.MI = application.MI;
                person.FirstName = application.FirstName;
                person.BadgeName = string.IsNullOrWhiteSpace(application.BadgeName)
                                       ? application.FirstName
                                       : application.BadgeName;
                person.Phone = application.FirmPhone;
                person.PhoneExt = application.FirmPhoneExt;
                person.User = application.User;
                person.OriginalPicture = application.Photo;
                person.ContentType = application.ContentType;
                person.CommunicationOption = application.CommunicationOption;
                person.ContactInformationRelease = application.ContactInformationRelease;
            }
            
            return person;
        }

        // Create or update the person's business address if exists
        private void UpdateAddress(Person person, Application application)
        {
            // check if the person already has the address
            var address = person.Addresses.Where(a => a.AddressType.Id == StaticIndexes.Address_Business.ToCharArray()[0]).FirstOrDefault();

            if (address == null)
            {
                address = new Address(application.FirmAddressLine1, application.FirmAddressLine2, application.FirmCity, application.FirmState, application.FirmZip, _addressTypeRepository.GetNullableById(StaticIndexes.Address_Business.ToCharArray()[0]), person);
                person.AddAddress(address);
            }
            else
            {
                // update the existing address
                address.Line1 = application.FirmAddressLine1;
                address.Line2 = application.FirmAddressLine2;
                address.City = application.FirmCity;
                address.State = application.FirmState;
                address.Zip = application.FirmZip;
            }
        }

        private void UpdateAssistant(Person person, Application application)
        {
            // transfer the assistant information
            var assistantType = _contactTypeRepository.GetNullableById('A');
            var assistant = person.Contacts.Where(a => a.ContactType == assistantType).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(application.AssistantFirstName) && !string.IsNullOrWhiteSpace(application.AssistantLastName) && (!string.IsNullOrWhiteSpace(application.AssistantPhone) || !string.IsNullOrWhiteSpace(application.AssistantEmail)))
            {
                if (assistant != null)
                {
                    assistant.FirstName = application.AssistantFirstName;
                    assistant.LastName = application.AssistantLastName;
                    assistant.Email = application.AssistantEmail;
                    assistant.Phone = application.AssistantPhone;
                }
                else
                {
                    var newAssistant = new Contact(application.AssistantFirstName, application.AssistantLastName, application.AssistantPhone, assistantType, person);
                    newAssistant.Email = application.AssistantEmail;

                    person.AddContact(newAssistant);
                }

            }
        }
        #endregion
    }
}