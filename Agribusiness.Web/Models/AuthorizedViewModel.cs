using System;
using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
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

        public static AuthorizedViewModel Create(IRepository repository, ISeminarService seminarService, string userId)
        {
            Check.Require(repository != null, "Repository is required.");

            // load the user
            var user = repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == userId.ToLower()).FirstOrDefault();
            if (user == null) throw new ArgumentException(string.Format("Unable to load user with id {0}", userId));

            var person = user.Person;

            // load seminar
            var seminar = seminarService.GetCurrent();

            var viewModel = new AuthorizedViewModel()
                                {
                                    Seminar = seminar,
                                    // only load application that applies to current seminar
                                    Application = user.Applications.Where(a => a.Seminar == seminar).FirstOrDefault(),
                                    SeminarPeople = person != null ? person.SeminarPeople.Where(a=>a.Seminar.Id != seminar.Id).ToList() : new List<SeminarPerson>(),
                                    SeminarPerson = person != null ? person.SeminarPeople.Where(a=> a.Seminar.Id == seminar.Id).FirstOrDefault() : null,
                                    Person = person
                                };

            return viewModel;
        }
    }
}