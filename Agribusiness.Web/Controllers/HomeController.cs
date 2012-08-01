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
        private readonly ISeminarService _seminarService;

        public HomeController(ISeminarService seminarService)
        {
            _seminarService = seminarService;
        }

        public ActionResult Index()
        {
            var viewModel = HomeViewModel.Create(_seminarService, RepositoryFactory.SiteRepository , SiteService.LoadSite(Site));
            return View(viewModel);
        }

        [UserOnly]
        public ActionResult Admin()
        {
            //var pendingApplications = Repository.OfType<Application>().Queryable.Where(a => a.IsPending).Count();
            //var peopleMissingPicture = Repository.OfType<Person>().Queryable.Where(a=>a.OriginalPicture == null).Count();
            //var firmsRequiringReview = Repository.OfType<Firm>().Queryable.Where(a => a.Review).Count();
            //var pendingInformationRequests = Repository.OfType<InformationRequest>().Queryable.Where(a => !a.Responded).Count();
            //var message = new StringBuilder();

            //if (pendingApplications > 0)
            //{
            //    message.Append(string.Format("There are {0} pending applications to review.<br/>", pendingApplications));
            //}

            //if (peopleMissingPicture > 0)
            //{
            //    message.Append(string.Format("There are {0} profiles that are missing pictures.<br/>", peopleMissingPicture));
            //}

            //if (firmsRequiringReview > 0)
            //{
            //    message.Append(string.Format("There are {0} firms waiting approval.", firmsRequiringReview));
            //}

            //if (pendingInformationRequests > 0)
            //{
            //    message.Append(string.Format("There are {0} pending information requests.", pendingInformationRequests));
            //}

            //return View(message);

            var viewModel = AdminIndexViewModel.Create(RepositoryFactory);
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

        public static AdminIndexViewModel Create(IRepositoryFactory repositoryFactory)
        {
            var viewModel = new AdminIndexViewModel()
                                {
                                    PendingInformationRequests = repositoryFactory.InformationRequestRepository.Queryable.Count(a => !a.Responded),
                                    PendingApplications = repositoryFactory.ApplicationRepository.Queryable.Count(a => a.IsPending),
                                    ApplicationsPaid = 0, PeopleMissingBiography = 0, PeopleMissingPhoto = 0, PeopleMissingHotel = 0
                                };

            return viewModel;
        }

    }
}
