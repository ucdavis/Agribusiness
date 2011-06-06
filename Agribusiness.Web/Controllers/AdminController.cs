using System.Web.Mvc;
using Agribusiness.Web.Controllers.Filters;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Admin class
    /// </summary>
    [UserOnly]
    public class AdminController : ApplicationController
    {
        public AdminController()
        {
        }

        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
    }
}
