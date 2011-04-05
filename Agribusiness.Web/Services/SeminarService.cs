using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Agribusiness.Web.Services
{
    public class SeminarService : ISeminarService
    {
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<Firm> _firmRepository;

        public SeminarService(IRepository<Seminar> seminarRepository, IRepository<Person> personRepository, IRepository<Firm> firmRepository)
        {
            _seminarRepository = seminarRepository;
            _personRepository = personRepository;
            _firmRepository = firmRepository;
        }

        public Seminar GetCurrent()
        {
            // get the seminar with the highest id
            var seminar = _seminarRepository.Queryable.Where(x => x.Id == (_seminarRepository.Queryable.Max(y => y.Id))).FirstOrDefault();

            return seminar;
        }

        public void CreateSeminarPerson(Application application)
        {
            var person = application.User.Person ?? new Person()
            {
                LastName = application.LastName,
                MI = application.MI,
                FirstName = application.FirstName,
                BadgeName = string.IsNullOrWhiteSpace(application.BadgeName) ? application.FirstName : application.BadgeName,
                Phone = application.FirmPhone,
                User = application.User,
                OriginalPicture = application.Photo,
                ContentType = application.ContentType
            };


            var firm = application.Firm ?? new Firm(application.FirmName, application.FirmDescription);

            var seminarPerson = new SeminarPerson()
            {
                Seminar = application.Seminar,
                Title = application.JobTitle,
                FirmCode = firm.FirmCode
            };

            person.AddSeminarPerson(seminarPerson);

            _firmRepository.EnsurePersistent(firm);
            _personRepository.EnsurePersistent(person);
        }
    }
}