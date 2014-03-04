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
        private readonly IRepositoryWithTypedId<ContactType, char> _contactTypeRepository;

        public SeminarService(IRepository<Seminar> seminarRepository, IRepository<Person> personRepository, IRepository<Firm> firmRepository, IRepositoryWithTypedId<AddressType, char> addressTypeRepository, IRepositoryWithTypedId<ContactType, char> contactTypeRepository)
        {
            _seminarRepository = seminarRepository;
            _personRepository = personRepository;
            _firmRepository = firmRepository;
            _addressTypeRepository = addressTypeRepository;
            _contactTypeRepository = contactTypeRepository;
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

        


    }
}