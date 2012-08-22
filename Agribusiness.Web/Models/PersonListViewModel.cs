using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
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
            Check.Require(personService != null, "personService is required.");

            var viewModel = new PersonListViewModel()
                                {
                                    People = personService.GetAllDisplayPeople(), 
                                    Seminar = SiteService.GetLatestSeminar(siteId),
                                    Site = SiteService.LoadSite(siteId, true)
                                };

            //var people = personRepository.GetAll();
            //viewModel.People = personService.ConvertToDisplayPeople(people);
            viewModel.SitePeople = personService.ConvertToDisplayPeople(viewModel.Site.People);

            // pull the invitation list
            var invitations = viewModel.Seminar.Invitations.Where(a => a.Seminar.Site.Id == siteId).Select(a => a.Person).ToList();
            // pull applications
            var applications = viewModel.Seminar.Applications.Where(a => a.Seminar.Site.Id == siteId).Select(a => a.User.Person).ToList();
            // pull seminar people
            var seminarPeople = viewModel.Seminar.SeminarPeople;

            foreach (var dp in viewModel.People)
            {
                var sp = seminarPeople.FirstOrDefault(a => a.Person == dp.Person);
                var application = applications.Any(a => a == dp.Person);
                var invited = invitations.Any(a => a == dp.Person);

                if (sp != null)
                {
                    if (sp.Paid)
                    {
                        dp.Registered = true;
                    }
                    else
                    {
                        dp.Accepted = true;
                    }
                }
                else if (application)
                {
                    dp.Applied = true;
                }
                else if (invited)
                {
                    dp.Invite = true;
                }
                else
                {
                    dp.Registered = false;
                    dp.Accepted = false;
                    dp.Applied = false;
                    dp.Invite = false;
                }
            }

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
        public bool Applied { get; set; }
        public bool Accepted { get; set; }
        public bool Registered { get; set; }

        public bool InSeminar { get { return Invite || Applied || Accepted || Registered; } }
    }
}