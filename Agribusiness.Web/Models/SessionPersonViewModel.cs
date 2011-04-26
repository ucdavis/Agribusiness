using System.Collections.Generic;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class SessionPersonViewModel
    {
        public IEnumerable<SeminarPerson> SeminarPeople { get; set; }
        public Session Session { get; set; }
        public SeminarPerson SeminarPerson { get; set; }

        public static SessionPersonViewModel Create(IRepository repository, Session session, SeminarPerson seminarPerson = null)
        {
            Check.Require(repository != null, "Repository is required.");

            var seminar = repository.OfType<Seminar>().GetNullableById(session.Seminar.Id);

            var viewModel = new SessionPersonViewModel()
                                {
                                    SeminarPeople = seminar.SeminarPeople,
                                    Session = session,
                                    SeminarPerson = seminarPerson ?? new SeminarPerson()
                                };

            return viewModel;
        }
    }
}