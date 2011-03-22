using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class PersonListViewModel
    {
        public List<DisplayPerson> People { get; set; }

        public static PersonListViewModel Create(IRepository<Person> personRepository, IRepository<Firm> firmRepository)
        {
            Check.Require(personRepository != null, "personRepository is required.");
            Check.Require(firmRepository != null, "firmRepository is required.");

            var test = firmRepository.Queryable.Where(firm => firmRepository.Queryable.GroupBy(a => a.FirmCode).Select(b => b.Max(c => c.Id)).Contains(firm.Id)).ToList();

            var viewModel = new PersonListViewModel(){People = new List<DisplayPerson>()};

            var people = personRepository.GetAll();
            var firms = GetListofFirms(firmRepository);

            foreach (var a in people)
            {
                var reg = a.GetLatestRegistration();
                if (reg != null)
                {
                    var firm = firms.Where(b => b.FirmCode == reg.FirmCode).FirstOrDefault();

                    viewModel.People.Add(new DisplayPerson(){Firm = firm, Person = a});
                }
                else
                {
                    viewModel.People.Add(new DisplayPerson(){Person = a});
                }
            }

            return viewModel;
        }

        private static List<Firm> GetListofFirms(IRepository<Firm> firmRepository)
        {
            // load firms
            var allFirms = firmRepository.GetAll();
            var firmCodes = allFirms.Select(a => a.FirmCode).Distinct();

            var firms = new List<Firm>();

            foreach (var a in firmCodes)
            {
                var firm = allFirms.Where(c => c.Id == allFirms.Where(b => b.FirmCode == a).Max(b => b.Id)).SingleOrDefault();

                if (firm != null) firms.Add(firm);
            }

            return firms;
        }
    }

    public class DisplayPerson
    {
        public Person Person { get; set; }
        public Firm Firm { get; set; }
    }
}