using System.Linq;
using System.Text;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;

namespace Agribusiness.Web.Controllers
{
    public class HomeController : ApplicationController
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            var viewModel = HomeViewModel.Create(RepositoryFactory.SiteRepository , SiteService.LoadSite(Site));
            return View(viewModel);
        }

        [UserOnly]
        public ActionResult Admin()
        {
            var viewModel = AdminIndexViewModel.Create(RepositoryFactory, Site);
            return View(viewModel);
        }

        public ActionResult About()
        {
            return View();
        }
    }

    public class AdminIndexViewModel
    {
        public int PendingInformationRequests { get; set; }
        public int PendingApplications { get; set; }
        public int ApplicationsPaid { get; set; }
        public int PeopleMissingBiography { get; set; }
        public int PeopleMissingPhoto { get; set; }
        public int PeopleMissingHotel { get; set; }

        public static AdminIndexViewModel Create(IRepositoryFactory repositoryFactory, string site)
        {
            var seminar = SiteService.GetLatestSeminar(site);

            var viewModel = new AdminIndexViewModel()
                                {
                                    PendingInformationRequests = repositoryFactory.InformationRequestRepository.Queryable.Where(a => a.Site.Id == site).Count(a => !a.Responded),
                                    PendingApplications = repositoryFactory.ApplicationRepository.Queryable.Where(a => a.Seminar.Id == seminar.Id).Count(a => a.IsPending),
                                    ApplicationsPaid = 0, PeopleMissingBiography = 0, PeopleMissingPhoto = 0, PeopleMissingHotel = 0
                                };

            return viewModel;
        }

    }
}
