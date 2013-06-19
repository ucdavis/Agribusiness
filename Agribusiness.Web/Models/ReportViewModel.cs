using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Report class
    /// </summary>
    public class ReportViewModel
    {
        public IEnumerable<Seminar> Seminars { get; set; }
        public Seminar Seminar { get; set; }
        public IEnumerable<Site> Sites { get; set; }
        public string Site { get; set; }

        public static ReportViewModel Create(IRepository repository, string siteId)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new ReportViewModel {Seminars = repository.OfType<Seminar>().GetAll(), Seminar = SiteService.GetLatestSeminar(siteId), Site = siteId};
            viewModel.Sites = repository.OfType<Site>().GetAll();
 
            return viewModel;
        }
    }
}