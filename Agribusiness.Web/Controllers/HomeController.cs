using System.Linq;
using System.Web.Mvc;
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
            viewModel.Files = RepositoryFactory.FileRepository.Queryable.Where(a => a.Home);
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
        public int RespondedInformationRequests { get; set; }

        public int PendingApplications { get; set; }
        public int ApprovedApplications { get; set; }
        public int DeniedApplications { get; set; }

        public int Registered { get; set; }
        
        public int PeopleMissingBiography { get; set; }
        public int PeopleMissingPhoto { get; set; }
        public int PeopleMissingHotel { get; set; }

        public static AdminIndexViewModel Create(IRepositoryFactory repositoryFactory, string site)
        {
            var seminar = SiteService.GetLatestSeminar(site);

            var viewModel = new AdminIndexViewModel()
                                {
                                    PendingInformationRequests = repositoryFactory.InformationRequestRepository.Queryable.Where(a => a.Site.Id == site).Count(a => !a.Responded),

                                    PendingApplications = repositoryFactory.ApplicationRepository.Queryable.Count(a => a.Seminar.Id == seminar.Id && a.IsPending),
                                    ApprovedApplications = repositoryFactory.ApplicationRepository.Queryable.Count(a => a.Seminar.Id == seminar.Id && !a.IsPending && a.IsApproved),
                                    DeniedApplications = repositoryFactory.ApplicationRepository.Queryable.Count(a => a.Seminar.Id == seminar.Id && !a.IsPending && !a.IsApproved),

                                    Registered = repositoryFactory.SeminarPersonRepository.Queryable.Count(a => a.Seminar.Id == seminar.Id && a.Paid), 
                                    PeopleMissingBiography = repositoryFactory.SeminarPersonRepository.Queryable.Count(a => a.Seminar.Id == seminar.Id && a.Paid && a.Person.Biography != null && a.Person.Biography != string.Empty),
                                    PeopleMissingPhoto = repositoryFactory.SeminarPersonRepository.Queryable.Count(a => a.Seminar.Id == seminar.Id && a.Paid && a.Person.OriginalPicture == null),
                                    PeopleMissingHotel = repositoryFactory.SeminarPersonRepository.Queryable.Count(a => a.Seminar.Id == seminar.Id && a.Paid && a.HotelConfirmation != null && a.HotelConfirmation != string.Empty)
                                };

            return viewModel;
        }

    }
}
