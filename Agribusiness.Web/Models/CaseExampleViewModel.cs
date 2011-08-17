using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class CaseExampleViewModel
    {
        public IEnumerable<CaseStudy> CaseStudies { get; set; }
        public Seminar CurrentSeminar { get; set; }

        public static CaseExampleViewModel Create(IRepository<CaseStudy> caseStudyRepository, ISeminarService seminarService )
        {
            Check.Require(caseStudyRepository != null, "caseStudyRepository is required.");
            Check.Require(seminarService != null, "seminarService is required.");

            var viewModel = new CaseExampleViewModel()
                                {
                                    CaseStudies = caseStudyRepository.Queryable.Where(a => a.IsPublic).OrderBy(a => a.Seminar.Year).ThenBy(a => a.Name),
                                    CurrentSeminar = seminarService.GetCurrent()
                                };

            return viewModel;
        }
    }
}