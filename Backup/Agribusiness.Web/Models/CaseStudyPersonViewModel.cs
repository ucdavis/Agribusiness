using System.Collections.Generic;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class CaseStudyPersonViewModel
    {
        public CaseStudy CaseStudy { get; set; }
        public int SeminarPersonId { get; set; }
        public IEnumerable<SeminarPerson> SeminarPeople { get; set; }

        public static CaseStudyPersonViewModel Create(IRepository repository, CaseStudy caseStudy)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new CaseStudyPersonViewModel(){CaseStudy = caseStudy, SeminarPeople = caseStudy.Seminar.SeminarPeople};

            return viewModel;
        }
    }
}