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
                                    DisplayPeople = personService.GetDisplayPeopleForSeminar(seminar.Id),
                                    PersonId = personId.HasValue ? personId.Value : -1
                                };

            return viewModel;
        }
    }
}