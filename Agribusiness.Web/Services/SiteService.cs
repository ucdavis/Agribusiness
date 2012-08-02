using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
using Microsoft.Practices.ServiceLocation;

namespace Agribusiness.Web.Services
{
    public static class SiteService
    {
        public static IRepositoryFactory RepositoryFactory = ServiceLocator.Current.GetInstance<IRepositoryFactory>();
        public static string SeminarKey = "{0}-Seminar";
        private static string SitesKey = "Sites";

        /// <summary>
        /// Loads Site from Cache if available, other goes to DB
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="forceReload">Force reload of the site</param>
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
        /// <param name="forceReload">Force reload of the site</param>
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
            HttpContext.Current.Cache[site.Id] = site;
            if (site.Seminars.Any())
            {
                HttpContext.Current.Cache[string.Format(SeminarKey, site.Id)] = site.Seminars.OrderByDescending(a => a.Year).FirstOrDefault();    
            }
        }

        /// <summary>
        /// Loads list of sites to determine domain
        /// </summary>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> LoadSiteDomains()
        {
            var sites = (List<KeyValuePair<string, string>>)HttpContext.Current.Cache[SitesKey];

            if (sites == null)
            {
                sites = RepositoryFactory.SiteRepository.Queryable.Select(a => new KeyValuePair<string, string>(a.Id, a.Subdomain)).ToList();
                HttpContext.Current.Cache[SitesKey] = sites;
            }

            return sites;
        }

        /// <summary>
        /// Recache the list of sites to determine domain
        /// </summary>
        public static void RecacheSiteDomains()
        {
            var sites = RepositoryFactory.SiteRepository.Queryable.Select(a => new KeyValuePair<string, string>(a.Id, a.Subdomain)).ToList();
            HttpContext.Current.Cache[SitesKey] = sites;   
        }
    }
}