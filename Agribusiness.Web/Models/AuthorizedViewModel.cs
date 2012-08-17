using System;
using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Resources;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// View model for authorized user home page
    /// </summary>
    public class AuthorizedViewModel
    {
        // this field should not be nullable
        public Seminar Seminar { get; set; }

        // below are nullable fields
        public Application Application { get; set; }
        public IEnumerable<SeminarPerson> SeminarPeople { get; set; }
        // seminar person for the current seminar
        public SeminarPerson SeminarPerson { get; set; }
        public Person Person { get; set; }

        public bool Invited { get; set; }

        public static AuthorizedViewModel Create(IRepository repository, string userId, string siteId)
        {
            Check.Require(repository != null, "Repository is required.");

            // load the user
            var user = repository.OfType<User>().Queryable.FirstOrDefault(a => a.LoweredUserName == userId.ToLower());
            if (user == null) throw new ArgumentException(string.Format("Unable to load user with id {0}", userId));

            var person = user.Person;

            // load seminar
            var seminar = SiteService.GetLatestSeminar(siteId);

            // has this person been invited to the current seminar?
            var invited = seminar.Invitations.Any(a => a.Person.User.LoweredUserName == userId.ToLower());

            var viewModel = new AuthorizedViewModel()
                                {
                                    Seminar = seminar,
                                    // only load application that applies to current seminar
                                    Application = user.Applications.FirstOrDefault(a => a.Seminar.Id == seminar.Id),
                                    SeminarPeople = person != null ? person.SeminarPeople.Where(a=>a.Seminar.Id != seminar.Id).ToList() : new List<SeminarPerson>(),
                                    SeminarPerson = person != null ? person.SeminarPeople.FirstOrDefault(a => a.Seminar.Id == seminar.Id) : null,
                                    Person = person,
                                    Invited = invited
                                };

            return viewModel;
        }
    }
}