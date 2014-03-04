using System.Collections.Generic;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class AddAttendeeViewModel
    {
        public Seminar Seminar { get; set; }
        public IEnumerable<DisplayPerson> DisplayPeople { get; set; }
        public int PersonId { get; set; }

        public static AddAttendeeViewModel Create(IRepository repository, IPersonService personService, Seminar seminar, int? personId = null)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(personService != null, "personService is required.");

            var viewModel = new AddAttendeeViewModel()
                                {
                                    Seminar = seminar,
                                    DisplayPeople = personService.GetDisplayPeopleNotInSeminar(seminar.Id),
                                    PersonId = personId.HasValue ? personId.Value : -1
                                };

            return viewModel;
        }
    }

    public class AddConfirmViewModel
    {
        public Seminar Seminar { get; set; }
        public Person Person { get; set; }
        public SeminarPerson SeminarPerson { get; set; }

        public IEnumerable<Firm> Firms { get; set; }

        public Firm Firm { get; set; }

        public static AddConfirmViewModel Create(IRepository repository, IFirmService firmService, Seminar seminar, Person person, SeminarPerson seminarPerson, Firm firm = null)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new AddConfirmViewModel()
                                {
                                    //Firms = firmService.GetAllFirms(),
                                    Seminar = seminar, Person = person, 
                                    SeminarPerson = seminarPerson ?? new SeminarPerson(),
                                    Firm = firm
                                };

            var firms = new List<Firm>(firmService.GetAllFirms());
            firms.Add(new Firm("Other", null, null));

            viewModel.Firms = firms;

            return viewModel;
        }
    }
}