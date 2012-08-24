using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.Controllers.Filters;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the EmailQueue class
    /// </summary>
    [UserOnly]
    public class EmailQueueController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public EmailQueueController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        //
        // GET: /EmailQueue/
        public ActionResult Index()
        {
            var emailqueueList = _repositoryFactory.VEmailQueueRepository.Queryable;

            return View(emailqueueList);
        }

        public ActionResult Flush()
        {

#if DEBUG
#else
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainDB"].ConnectionString))
            {
                conn.Open();

                // create command
                var cmd = new SqlCommand("usp_SendEmails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 99999;
                cmd.ExecuteNonQuery();
            }
#endif

            Message = "Email queue has been processed.";
            return RedirectToAction("Index");
        }
    }

}
