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
        public IEnumerable<DisplayPerson> SitePeople { get; set; } 
        public bool Desc { get; set; }
        public string SortBy { get; set; }
        public Seminar Seminar { get; set; }
        public Site Site { get; set; }

        public static PersonListViewModel Create(IRepository<Person> personRepository, IPersonService personService, string siteId)
        {
            Check.Require(personRepository != null, "personRepository is required.");
            Check.Require(personService != null, "personService is required.");

            var viewModel = new PersonListViewModel()
                                {
                                    People = personService.GetAllDisplayPeople(), 
                                    Seminar = SiteService.GetLatestSeminar(siteId),
                                    Site = SiteService.LoadSite(siteId, true)
                                };

            viewModel.SitePeople = personService.ConvertToDisplayPeople(viewModel.Site.People);

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