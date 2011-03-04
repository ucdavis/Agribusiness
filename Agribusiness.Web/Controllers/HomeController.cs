using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using UCDArch.Web.Attributes;
using System;

namespace Agribusiness.Web.Controllers
{
    [HandleTransactionsManually]
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

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
