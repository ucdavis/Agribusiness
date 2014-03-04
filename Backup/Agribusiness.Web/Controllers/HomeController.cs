using System.Linq;
using System.Text;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Services;
using UCDArch.Web.Attributes;

namespace Agribusiness.Web.Controllers
{
    [HandleTransactionsManually]
    public class HomeController : ApplicationController
    {
        private readonly ISeminarService _seminarService;

        public HomeController(ISeminarService seminarService)
        {
            _seminarService = seminarService;
        }

        public ActionResult Index()
        {
            var seminar = _seminarService.GetCurrent();

            return View(seminar);
        }

        [UserOnly]
        public ActionResult Admin()
        {
            var pendingApplications = Repository.OfType<Application>().Queryable.Where(a => a.IsPending).Count();
            var peopleMissingPicture = Repository.OfType<Person>().Queryable.Where(a=>a.OriginalPicture == null).Count();
            var firmsRequiringReview = Repository.OfType<Firm>().Queryable.Where(a => a.Review).Count();
            var pendingInformationRequests = Repository.OfType<InformationRequest>().Queryable.Where(a => !a.Responded).Count();
            var message = new StringBuilder();

            if (pendingApplications > 0)
            {
                message.Append(string.Format("There are {0} pending applications to review.<br/>", pendingApplications));
            }

            if (peopleMissingPicture > 0)
            {
                message.Append(string.Format("There are {0} profiles that are missing pictures.<br/>", peopleMissingPicture));
            }

            if (firmsRequiringReview > 0)
            {
                message.Append(string.Format("There are {0} firms waiting approval.", firmsRequiringReview));
            }

            if (pendingInformationRequests > 0)
            {
                message.Append(string.Format("There are {0} pending information requests.", pendingInformationRequests));
            }

            return View(message);
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
