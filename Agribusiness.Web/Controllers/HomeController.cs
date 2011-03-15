using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using UCDArch.Web.Attributes;
using System;

namespace Agribusiness.Web.Controllers
{
    [HandleTransactionsManually]
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            return View();
        }

        [UserOnly]
        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [Transaction]
        public ActionResult LoadImages()
        {
            var people = Repository.OfType<Person>().GetAll();

            return View(people.First());
        }

        [Transaction]
        public ActionResult LoadPicture(int id)
        {
            var person = Repository.OfType<Person>().GetById(id);

            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));

            return File(person.Picture, "image/jpg");
        }
    }
}
