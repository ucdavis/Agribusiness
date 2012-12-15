using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class AttendeeListViewModel
    {
        public Seminar Seminar { get; set; }
        public IEnumerable<DisplayPerson> SeminarPeople { get; set; }

        public static AttendeeListViewModel Create(Seminar seminar, IPersonService personService, string siteId)
        {
            Check.Require(seminar != null, "seminar is required.");
            Check.Require(personService != null, "personService is required.");

            var viewModel = new AttendeeListViewModel()
                                {
                                    Seminar = seminar,
                                };

            // pull the invitation list
            var invitations = viewModel.Seminar.Invitations.Where(a => a.Seminar.Site.Id == siteId).Select(a => a.Person).ToList();
            // pull applications
            var applications = viewModel.Seminar.Applications.Where(a => a.Seminar.Site.Id == siteId).ToList();
            // pull seminar people
            var seminarPeople = viewModel.Seminar.SeminarPeople;

            var people = new List<DisplayPerson>();

            people.AddRange(DetermineParticipation(personService, seminarPeople.Where(a => a.Paid).Select(a => a.Person).ToList(), siteId, registered: true));
            people.AddRange(DetermineParticipation(personService, seminarPeople.Where(a => !a.Paid).Select(a => a.Person).ToList(), siteId, accepted: true));
            people.AddRange(DetermineParticipation(personService, applications.Where(a => !a.IsPending && !a.IsApproved).Select(a => a.User.Person).ToList(), siteId, denied: true));
            people.AddRange(DetermineParticipation(personService, applications.Where(a => a.IsPending).Select(a => a.User.Person).ToList(), siteId, applied: true));
            people.AddRange(DetermineParticipation(personService, invitations.Where(a => !people.Select(b => b.Person).Contains(a)).ToList(), siteId, invite: true));

            viewModel.SeminarPeople = people;

            return viewModel;
        }

        private static List<DisplayPerson> DetermineParticipation(IPersonService personService, List<Person> people, string site, bool invite = false, bool applied = false, bool accepted = false, bool registered = false, bool denied = false)
        {
            var tmp = personService.ConvertToDisplayPeople(people, site);
            var result = new List<DisplayPerson>();
            foreach (var p in tmp)
            {
                p.Registered = registered;
                p.Applied = applied;
                p.Accepted = accepted;
                p.Invite = invite;
                p.Denied = denied;

                result.Add(p);
            }

            return result;
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
        public bool Denied { get; set; }
        public bool Registered { get; set; }

        public bool InSeminar { get { return Invite || Applied || Accepted || Registered || Denied; } }
    }
}