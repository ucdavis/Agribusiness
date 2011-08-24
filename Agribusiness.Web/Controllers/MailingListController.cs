using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the MailingList class
    /// </summary>
    [UserOnly]
    public class MailingListController : ApplicationController
    {
	    private readonly IRepository<MailingList> _mailinglistRepository;

        public MailingListController(IRepository<MailingList> mailinglistRepository)
        {
            _mailinglistRepository = mailinglistRepository;
        }
    
        /// <summary>
        /// List of Mailing Lists
        /// </summary>
        /// <param name="seminarId">Optional: Limits list to Seminar</param>
        /// <returns></returns>
        public ActionResult Index(int? seminarId)
        {
            var mailinglistList = seminarId.HasValue ? _mailinglistRepository.Queryable.Where(a=>a.Seminar.Id == seminarId) : _mailinglistRepository.Queryable;

            ViewBag.SeminarId = seminarId;

            return View(mailinglistList);
        }

        //
        // GET: /MailingList/Details/5
        public ActionResult Details(int id)
        {
            var mailinglist = _mailinglistRepository.GetNullableById(id);

            if (mailinglist == null) return RedirectToAction("Index");

            return View(mailinglist);
        }

        //
        // GET: /MailingList/Create
        public ActionResult Create(int? seminarId)
        {
			var viewModel = MailingListViewModel.Create(Repository, seminarId:seminarId);
            
            return View(viewModel);
        } 

        //
        // POST: /MailingList/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(MailingList mailinglist, int? seminarId)
        {
            if (ModelState.IsValid)
            {
                _mailinglistRepository.EnsurePersistent(mailinglist);

                Message = "MailingList Created Successfully";

                //return RedirectToAction("Index");
                return this.RedirectToAction(a => a.Index(seminarId));
            }

            var viewModel = MailingListViewModel.Create(Repository, mailinglist, seminarId);
            viewModel.MailingList = mailinglist;

            return View(viewModel);
        }

        //
        // GET: /MailingList/Edit/5
        public ActionResult Edit(int id)
        {
            var mailinglist = _mailinglistRepository.GetNullableById(id);

            if (mailinglist == null) return RedirectToAction("Index");

			var viewModel = MailingListViewModel.Create(Repository, mailinglist, mailinglist.Seminar != null ? mailinglist.Seminar.Id : (int?)null);

			return View(viewModel);
        }
        
        //
        // POST: /MailingList/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, MailingList mailinglist)
        {
            if (ModelState.IsValid)
            {
                _mailinglistRepository.EnsurePersistent(mailinglist);

                Message = "MailingList Edited Successfully";

                return this.RedirectToAction(a => a.Index(mailinglist.Seminar != null ? mailinglist.Seminar.Id : (int?)null));
            }
            
            var viewModel = MailingListViewModel.Create(Repository, mailinglist, mailinglist.Seminar != null ? mailinglist.Seminar.Id : (int?)null);

            return View(viewModel);
        }
        
        //
        // GET: /MailingList/Delete/5 
        public ActionResult Delete(int id)
        {
			var mailinglist = _mailinglistRepository.GetNullableById(id);

            if (mailinglist == null) return RedirectToAction("Index");

            return View(mailinglist);
        }

        //
        // POST: /MailingList/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, MailingList mailinglist)
        {
			var mailinglistToDelete = _mailinglistRepository.GetNullableById(id);

            if (mailinglistToDelete == null) return RedirectToAction("Index");

            _mailinglistRepository.Remove(mailinglistToDelete);

            Message = "MailingList Removed Successfully";

            return RedirectToAction("Index");
        }

        #region Ajax Actions
        /// <summary>
        /// Gets a list of mailing list to show in a drop down
        /// </summary>
        /// <param name="seminarId"></param>
        /// <returns></returns>
        public JsonNetResult GetList(int seminarId)
        {
            var result = _mailinglistRepository.Queryable.Where(a => a.Seminar == null || a.Seminar.Id == seminarId);

            return new JsonNetResult(result.Select(a => new {Id=a.Id, Label=a.Name}));
        }
        /// <summary>
        /// Add a person into the mailing list
        /// </summary>
        /// <param name="mailingListId"></param>
        /// <param name="personId"></param>
        /// <returns></returns>
        public JsonNetResult AddToList(int mailingListId, int personId)
        {
            var mailingList = _mailinglistRepository.GetNullableById(mailingListId);
            var person = Repository.OfType<Person>().GetNullableById(personId);

            if (mailingList == null || person == null)
            {
                return new JsonNetResult(false);
            }

            if (!mailingList.People.Contains(person))
            {
                mailingList.People.Add(person);
                _mailinglistRepository.EnsurePersistent(mailingList);
            }

            return new JsonNetResult(true);
        }
        #endregion
    }
}
