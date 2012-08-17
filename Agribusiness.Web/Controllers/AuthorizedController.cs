using System.Web.Mvc;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Home controller for authorized users.
    /// </summary>
    [MembershipUserOnly]
    public class AuthorizedController : ApplicationController
    {
        public ActionResult Index()
        {
            var viewModel = AuthorizedViewModel.Create(Repository, CurrentUser.Identity.Name, Site);

            return View(viewModel);
        }
    }
}
