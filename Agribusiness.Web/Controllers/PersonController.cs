using System.Web.Mvc;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Person class
    /// </summary>
    public class PersonController : ApplicationController
    {
	    private readonly IRepository<Person> _personRepository;

        public PersonController(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }
    
        //
        // GET: /Person/
        public ActionResult Index()
        {
            var personList = _personRepository.Queryable;

            return View(personList);
        }

    }
}
