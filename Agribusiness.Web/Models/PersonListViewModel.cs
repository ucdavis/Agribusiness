using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class PersonListViewModel
    {
        public IEnumerable<DisplayPerson> People { get; set; }

        public static PersonListViewModel Create(IRepository<Person> personRepository, IPersonService personService)
        {
            Check.Require(personRepository != null, "personRepository is required.");
            Check.Require(personService != null, "personService is required.");

            var viewModel = new PersonListViewModel() { People = personService.GetAllDisplayPeople() };

            return viewModel;
        }


    }

    public class DisplayPerson
    {
        public Person Person { get; set; }
        public string Title { get; set; }
        public Firm Firm { get; set; }

        public Seminar Seminar { get; set; }

        public bool Invite { get; set; }
        public bool Registered { get; set; }
    }
}