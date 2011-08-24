using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using MvcContrib;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the MailingList class
    /// </summary>
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
    }
}
