using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;

namespace Agribusiness.Web.Models
{
    public class HomeViewModel
    {
        public Seminar Seminar { get; set; }
        public Site Site { get; set; }

        public IEnumerable<Site> Sites { get; set; }

        public static HomeViewModel Create(ISeminarService seminarService, IRepositoryWithTypedId<Site, string> siteRepository, Site site)
        {
            var viewModel = new HomeViewModel()
                                {
                                    Site = site
                                };

            if (site == null)
            {
                viewModel.Sites = siteRepository.Queryable.Where(a => a.IsActive).ToList();
            }
            else
            {
                // load the current seminar
                viewModel.Seminar = SiteService.GetLatestSeminar(site.Id);
            }

            return viewModel;
        }

    }
}