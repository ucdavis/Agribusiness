using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using UCDArch.Web.Helpers;
using File = Agribusiness.Core.Domain.File;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the File class
    /// </summary>
    public class FileController : ApplicationController
    {
        private readonly IRepositoryFactory _repositoryFactory;
        private readonly IPersonService _personService;

        public FileController(IRepositoryFactory repositoryFactory, IPersonService personService)
        {
            _repositoryFactory = repositoryFactory;
            _personService = personService;
        }

        [UserOnly]
        public ActionResult Create(int seminarId)
        {
            return View(FileViewModel.Create(_repositoryFactory, seminarId));
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Create(int seminarId, File file, HttpPostedFileBase uploadedFile)
        {
            ModelState.Clear();

            file.Seminar = SiteService.GetLatestSeminar(Site);
            if (uploadedFile != null)
            {
                var reader = new BinaryReader(uploadedFile.InputStream);
                var data = reader.ReadBytes(uploadedFile.ContentLength);
                file.Contents= data;
                file.ContentType = uploadedFile.ContentType;
                file.FileName = uploadedFile.FileName;
            }

            file.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                _repositoryFactory.FileRepository.EnsurePersistent(file);
                Message = "File has been saved to seminar";
                return RedirectToAction("Edit", "Seminar", new {id = seminarId});
            }

            return View(FileViewModel.Create(_repositoryFactory, seminarId, file));
        }

        public ActionResult Edit(int id)
        {
            var file = _repositoryFactory.FileRepository.GetNullableById(id);

            if (file == null)
            {
                Message = "Unable to locate file.";
                return RedirectToAction("Index", "Seminar");
            }

            return View(FileViewModel.Create(_repositoryFactory, file.Seminar.Id, file));
        }

        [UserOnly]
        [HttpPost]
        public ActionResult Edit(int id, File file, HttpPostedFileBase uploadedFile)
        {
            return View();
        }

        public FileResult Download(int id)
        {
            var file = _repositoryFactory.FileRepository.GetNullableById(id);

            if (file.Public && _personService.HasAccess(CurrentUser.Identity.Name, file.Seminar, false))
            {
                return File(file.Contents, file.ContentType, file.FileName);
            }
            
            if (!file.Public)
            {
                return File(file.Contents, file.ContentType, file.FileName);
            }

            return File(new byte[0], string.Empty);
        }
    }
}
