using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;

namespace Agribusiness.Web.Services
{
    public class PersonService : IPersonService
    {
        private readonly IRepository<Firm> _firmRepository;

        public PersonService(IRepository<Firm> firmRepository)
        {
            _firmRepository = firmRepository;
        }


        public DisplayPerson GetDisplayPerson(Person person, IList<Firm> firms = null)
        {
            // just make sure we have a list of firms
            firms = firms ?? _firmRepository.GetAll();


            throw new NotImplementedException();
        }
    }
}