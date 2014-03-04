using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the CaseStudy class
    /// </summary>
    public class CaseStudyViewModel
    {
        public Seminar Seminar { get; set; }
        public CaseStudy CaseStudy { get; set; }

        public static CaseStudyViewModel Create(IRepository repository, Seminar seminar, CaseStudy caseStudy = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new CaseStudyViewModel
                                {
                                    Seminar = seminar,
                                    CaseStudy = caseStudy ?? new CaseStudy()
                                };
 
            return viewModel;
        }
    }
}