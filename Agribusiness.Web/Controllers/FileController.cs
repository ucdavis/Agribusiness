using System.IO;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Repositories;
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

        public FileController(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        public ActionResult Create(int seminarId)
        {
            return View(FileViewModel.Create(_repositoryFactory, seminarId));
        }

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
    }

    public class FileViewModel
    {
        public File File { get; set; }

        public static FileViewModel Create(IRepositoryFactory repositoryFactory, int seminarId, File file = null)
        {
            var viewModel = new FileViewModel()
                                {
                                    File = file ?? new File() { Seminar = repositoryFactory.SeminarRepository.GetNullableById(seminarId)}
                                };

            return viewModel;
        }

    }
}
