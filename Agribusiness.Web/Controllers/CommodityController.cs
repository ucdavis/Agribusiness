using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Commodity class
    /// </summary>
    [UserOnly]
    public class CommodityController : ApplicationController
    {
	    private readonly IRepository<Commodity> _commodityRepository;

        public CommodityController(IRepository<Commodity> commodityRepository)
        {
            _commodityRepository = commodityRepository;
        }
    
        //
        // GET: /Commodity/
        public ActionResult Index()
        {
            var commodityList = _commodityRepository.Queryable.Where(a=>a.IsActive);

            return View(commodityList);
        }

        //
        // GET: /Commodity/Create
        public ActionResult Create()
        {
			var viewModel = CommodityViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /Commodity/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Commodity commodity)
        {
            var commodityToCreate = new Commodity();

            TransferValues(commodity, commodityToCreate);

            if (ModelState.IsValid)
            {
                _commodityRepository.EnsurePersistent(commodityToCreate);

                Message = "Commodity Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = CommodityViewModel.Create(Repository);
                viewModel.Commodity = commodity;

                return View(viewModel);
            }
        }

        //
        // GET: /Commodity/Edit/5
        public ActionResult Edit(int id)
        {
            var commodity = _commodityRepository.GetNullableById(id);

            if (commodity == null) return RedirectToAction("Index");

			var viewModel = CommodityViewModel.Create(Repository);
			viewModel.Commodity = commodity;

			return View(viewModel);
        }
        
        //
        // POST: /Commodity/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Commodity commodity)
        {
            var commodityToEdit = _commodityRepository.GetNullableById(id);

            if (commodityToEdit == null) return RedirectToAction("Index");

            TransferValues(commodity, commodityToEdit);

            if (ModelState.IsValid)
            {
                _commodityRepository.EnsurePersistent(commodityToEdit);

                Message = "Commodity Edited Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = CommodityViewModel.Create(Repository);
                viewModel.Commodity = commodity;

                return View(viewModel);
            }
        }
        
        //
        // GET: /Commodity/Delete/5 
        public ActionResult Delete(int id)
        {
			var commodity = _commodityRepository.GetNullableById(id);

            if (commodity == null) return RedirectToAction("Index");

            return View(commodity);
        }

        //
        // POST: /Commodity/Delete/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Delete(int id, Commodity commodity)
        {
			var commodityToDelete = _commodityRepository.GetNullableById(id);

            if (commodityToDelete == null) return RedirectToAction("Index");

            _commodityRepository.Remove(commodityToDelete);

            Message = "Commodity Removed Successfully";

            return RedirectToAction("Index");
        }
        
        /// <summary>
        /// Transfer editable values from source to destination.  Use of AutoMapper is recommended
        /// </summary>
        private static void TransferValues(Commodity source, Commodity destination)
        {
            throw new NotImplementedException();
        }

    }
}
