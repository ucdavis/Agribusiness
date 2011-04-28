using System.Configuration;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class MySeminarViewModel
    {
        public SeminarPerson SeminarPerson { get; set; }
        public Seminar Seminar { get; set; }

        public string CrpLink { get; set; }

        public static MySeminarViewModel Create(IRepository repository, SeminarPerson seminarPerson)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new MySeminarViewModel()
                                {
                                    SeminarPerson =  seminarPerson,
                                    Seminar = seminarPerson.Seminar,
                                    CrpLink = string.Format(ConfigurationManager.AppSettings["CrpLink"], seminarPerson.Seminar.RegistrationId, seminarPerson.ReferenceId)
                                };

            return viewModel;
        }
    }
}