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

            if (site == null || !string.IsNullOrEmpty(siteId))
            {
                site = RepositoryFactory.SiteRepository.Queryable.FirstOrDefault(a => a.Id == siteId);

                if (site != null)
                {
                    System.Web.HttpContext.Current.Cache[siteId] = site;
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
                var site = RepositoryFactory.SiteRepository.Queryable.FirstOrDefault(a => a.Id == siteId);
                seminar = site.Seminars.OrderByDescending(a => a.End).FirstOrDefault();

                System.Web.HttpContext.Current.Cache[string.Format(SeminarKey, siteId)] = seminar;
            }

            return seminar;
        }
    }
}