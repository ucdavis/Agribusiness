using System.Collections.Generic;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class AttendeeListViewModel
    {
        public Seminar Seminar { get; set; }
        public IEnumerable<DisplayPerson> DisplayPeople { get; set; }

        public static AttendeeListViewModel Create(Seminar seminar, IPersonService personService)
        {
            Check.Require(seminar != null, "seminar is required.");
            Check.Require(personService != null, "personService is required.");

            var viewModel = new AttendeeListViewModel()
                                {
                                    Seminar = seminar,
                                    DisplayPeople = personService.GetDisplayPeopleForSeminar(seminar.Id)
                                };

            return viewModel;
        }
    }
}