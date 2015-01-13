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

        public static CaseExampleViewModel Create(IRepository<CaseStudy> caseStudyRepository, string site)
        {
            Check.Require(caseStudyRepository != null, "caseStudyRepository is required.");

            var viewModel = new CaseExampleViewModel()
                                {
                                    CaseStudies = caseStudyRepository.Queryable.Where(a => a.IsPublic && a.Seminar.Site.Id == site).OrderBy(a => a.Seminar.Year).ThenBy(a => a.Name),
                                    CurrentSeminar = SiteService.GetLatestSeminar(site)
                                };

            return viewModel;
        }
    }
}