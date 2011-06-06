using System;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using AutoMapper;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Template class
    /// </summary>
    [UserOnly]
    public class TemplateController : ApplicationController
    {
	    private readonly IRepository<Template> _templateRepository;

        public TemplateController(IRepository<Template> templateRepository)
        {
            _templateRepository = templateRepository;
        }
    
        //
        // GET: /Template/
        public ActionResult Index()
        {
            var templateList = _templateRepository.Queryable;

            return View(templateList);
        }

        //
        // GET: /Template/Details/5
        public ActionResult Details(int id)
        {
            var template = _templateRepository.GetNullableById(id);

            if (template == null) return RedirectToAction("Index");

            return View(template);
        }

        //
        // GET: /Template/Create
        public ActionResult Create()
        {
			var viewModel = TemplateViewModel.Create(Repository);
            
            return View(viewModel);
        } 

        //
        // POST: /Template/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create(Template template)
        {
            var templateToCreate = new Template();

            Mapper.Map(template, templateToCreate);

            if (ModelState.IsValid)
            {
                _templateRepository.EnsurePersistent(templateToCreate);

                Message = "Template Created Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = TemplateViewModel.Create(Repository);
                viewModel.Template = template;

                return View(viewModel);
            }
        }

        //
        // GET: /Template/Edit/5
        public ActionResult Edit(int id)
        {
            var template = _templateRepository.GetNullableById(id);

            if (template == null) return RedirectToAction("Index");

			var viewModel = TemplateViewModel.Create(Repository);
			viewModel.Template = template;

			return View(viewModel);
        }
        
        //
        // POST: /Template/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, Template template)
        {
            var templateToEdit = _templateRepository.GetNullableById(id);

            if (templateToEdit == null) return RedirectToAction("Index");

            Mapper.Map(template, templateToEdit);

            if (ModelState.IsValid)
            {
                _templateRepository.EnsurePersistent(templateToEdit);

                Message = "Template Edited Successfully";

                return RedirectToAction("Index");
            }
            else
            {
				var viewModel = TemplateViewModel.Create(Repository);
                viewModel.Template = template;

                return View(viewModel);
            }
        }
    }
}
