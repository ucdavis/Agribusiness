using System;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using AutoMapper;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.ActionResults;
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
        private readonly IRepositoryWithTypedId<NotificationType, string> _notificationTypeRepository;

        public TemplateController(IRepository<Template> templateRepository, IRepositoryWithTypedId<NotificationType, string> notificationTypeRepository)
        {
            _templateRepository = templateRepository;
            _notificationTypeRepository = notificationTypeRepository;
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
        [ValidateInput(false)]
        public ActionResult Create(Template template)
        {
            var templateToCreate = new Template();

            Mapper.Map(template, templateToCreate);

            if (ModelState.IsValid)
            {
                // inactivate all old templates
                foreach (var t in _templateRepository.Queryable.Where(a => a.NotificationType == templateToCreate.NotificationType))
                {
                    t.IsActive = false;
                    _templateRepository.EnsurePersistent(t);
                }

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
        [ValidateInput(false)]
        public ActionResult Edit(int id, Template template)
        {
            var templateToEdit = new Template();

            Mapper.Map(template, templateToEdit);

            if (ModelState.IsValid)
            {
                // inactivate all old templates
                foreach (var t in _templateRepository.Queryable.Where(a => a.NotificationType == templateToEdit.NotificationType))
                {
                    t.IsActive = false;
                    _templateRepository.EnsurePersistent(t);
                }

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

        /// <summary>
        /// Load the text of the template, by notification type id
        /// </summary>
        /// <param name="id">Notification Type Id</param>
        /// <returns></returns>
        [HttpPost]
        public JsonNetResult LoadTemplate(string id)
        {
            var template = _templateRepository.Queryable.Where(a => a.NotificationType.Id == id && a.IsActive).OrderByDescending(a => a.Id).FirstOrDefault();

            if (template == null)
            {
                return new JsonNetResult(string.Empty);
            }

            return new JsonNetResult(template.BodyText);
        }
    }
}
