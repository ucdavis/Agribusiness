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

        public SeminarService(IRepository<Seminar> seminarRepository)
        {
            _seminarRepository = seminarRepository;
        }

        public Seminar GetCurrent()
        {
            // get the seminar with the highest id
            var seminar = _seminarRepository.Queryable.Where(x => x.Id == (_seminarRepository.Queryable.Max(y => y.Id))).FirstOrDefault();

            return seminar;
        }
    }
}