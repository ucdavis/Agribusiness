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

        public static Site LoadSite(string siteId)
        {
            var site = (Site)System.Web.HttpContext.Current.Cache[siteId];

            if (site == null && !string.IsNullOrEmpty(siteId))
            {
                site = RepositoryFactory.SiteRepository.Queryable.FirstOrDefault(a => a.Id == siteId);
                
                if (site != null)
                {
                    System.Web.HttpContext.Current.Cache[siteId] = site;

                    if (site.Seminars != null && site.Seminars.Count > 0)
                    {
                        var seminar = site.Seminars.OrderByDescending(a => a.Year).FirstOrDefault();
                        System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, siteId)] = seminar;
                    }
                }
            }
            else if (site != null)
            {
                // check the seminar
                var seminar = (Seminar)System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, siteId)];

                if (seminar == null)
                {
                    seminar = RepositoryFactory.SeminarRepository.Queryable.Where(a => a.Site.Id == site.Id).OrderByDescending(a => a.Year).FirstOrDefault();
                    System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, siteId)] = seminar;
                }
            }

            return site;
        }

        public static void CacheSite(Site site)
        {
            System.Web.HttpContext.Current.Cache[site.Id] = site;
            System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, site.Id)] = site.Seminars.OrderByDescending(a => a.Year).FirstOrDefault();
        }

        public static Seminar GetLatestSeminar(string siteId)
        {
            var seminar = (Seminar) System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, siteId)];

            if (seminar == null)
            {
                // try to load the seminar
                var site = LoadSite(siteId);
                // try again 
                seminar = (Seminar) System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, siteId)];
            }

            return seminar;
        }
    }
}