using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Services
{
    public class SeminarService : ISeminarService
    {
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Firm> _firmRepository;
        private readonly IRepositoryWithTypedId<AddressType, char> _addressTypeRepository;

        public SeminarService(IRepository<Seminar> seminarRepository, IRepository<Person> personRepository, IRepository<Firm> firmRepository, IRepositoryWithTypedId<AddressType, char> addressTypeRepository)
        {
            _seminarRepository = seminarRepository;
            _personRepository = personRepository;
            _firmRepository = firmRepository;
            _addressTypeRepository = addressTypeRepository;
        }

        public Seminar GetCurrent()
        {
            // get the seminar with the highest id
            //var seminar = _seminarRepository.Queryable.Where(x => x.Id == (_seminarRepository.Queryable.Max(y => y.Id))).FirstOrDefault();

            // get the max year
            var year = _seminarRepository.Queryable.Max(x => x.Year);

            // get the max id with the max year
            var maxid = _seminarRepository.Queryable.Where(x => x.Year == year).Max(x => x.Id);

            // get that seminar
            var seminar = _seminarRepository.Queryable.Where(x => x.Id == maxid).FirstOrDefault();

            return seminar;
        }

        public Person CreateSeminarPerson(Application application, ModelStateDictionary modelState)
        {
            var person = application.User.Person ?? new Person()
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

            var firm = application.Firm ?? new Firm(application.FirmName, application.FirmDescription);

            var seminarPerson = new SeminarPerson()
            {
                Seminar = application.Seminar,
                Title = application.JobTitle,
                Firm = firm,
                Commodities = new List<Commodity>(application.Commodities)
            };

            person.AddSeminarPerson(seminarPerson);

            // add in the address
            var addrType = _addressTypeRepository.GetNullableById(StaticIndexes.Address_Business.ToCharArray()[0]);
            var address = new Address(application.FirmAddressLine1, application.FirmAddressLine2, application.FirmCity
                                      , application.FirmState, application.FirmZip, addrType, person);
            person.AddAddress(address);

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


    }
}