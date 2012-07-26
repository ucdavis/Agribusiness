using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
using Microsoft.Practices.ServiceLocation;

namespace Agribusiness.Web.Services
{
    public static class SiteService
    {
        public static IRepositoryFactory RepositoryFactory = ServiceLocator.Current.GetInstance<IRepositoryFactory>();
        public static string SeminarKey = "{0}-Seminar";

        /// <summary>
        /// Loads Site from Cache if available, other goes to DB
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static Site LoadSite(string siteId, bool forceReload = false)
        {
            var site = (Site)System.Web.HttpContext.Current.Cache[siteId];

            if (site == null || !string.IsNullOrEmpty(siteId) || forceReload)
            {
                site = RepositoryFactory.SiteRepository.Queryable.FirstOrDefault(a => a.Id == siteId);

                if (site != null)
                {
                    ReCacheSite(site);
                }
            }

            return site;
        }

        /// <summary>
        /// Gets a site's latest seminar from cache if available, otherwise go to db
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public static Seminar GetLatestSeminar(string siteId, bool forceReload = false)
        {
            var seminar = (Seminar) System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, siteId)];

            if (seminar == null || forceReload)
            {
                var site = RepositoryFactory.SiteRepository.Queryable.FirstOrDefault(a => a.Id == siteId);

                if (site != null)
                {
                    seminar = site.Seminars.OrderByDescending(a => a.End).FirstOrDefault();
                    ReCacheSite(site);
                }
            }

            return seminar;
        }

        /// <summary>
        /// Updates cache for a site
        /// </summary>
        /// <param name="site"></param>
        public static void ReCacheSite(Site site)
        {
            System.Web.HttpContext.Current.Cache[site.Id] = site;
            System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, site.Id)] = site.Seminars.OrderByDescending(a => a.Year).FirstOrDefault();
        }
    }
}