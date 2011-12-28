using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Services;
using Ionic.Zip;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Export class
    /// </summary>
    [UserOnly]
    public class ExportController : ApplicationController
    {
        private readonly IRepository<SeminarPerson> _seminarPersonRepository;
        private readonly ISeminarService _seminarService;

        public ExportController(IRepository<SeminarPerson> seminarPersonRepository, ISeminarService seminarService )
        {
            _seminarPersonRepository = seminarPersonRepository;
            _seminarService = seminarService;
        }

        //
        // GET: /Export/
        public FileResult Index()
        {
            var seminar = _seminarService.GetCurrent();

            var people = _seminarPersonRepository.Queryable.Where(a => a.Seminar == seminar && a.Paid).ToList();

            var stream = new MemoryStream();

            // create the zip file
            using (var zip = new ZipFile())
            {
                foreach (var person in people)
                {
                    if (person.Person != null && person.Person.OriginalPicture != null)
                    {
                        zip.AddEntry(string.Format("{0}.{1}.{2}", person.Person.LastName.Trim(), person.Person.FirstName.Trim(), ExtractExtension(person.Person.ContentType)), person.Person.OriginalPicture);

                        if (person.Person.MainProfilePicture != null)
                        {
                            zip.AddEntry(string.Format("{0}.{1}-profile.{2}", person.Person.LastName.Trim(), person.Person.FirstName.Trim(), ExtractExtension(person.Person.ContentType)), person.Person.MainProfilePicture);    
                        }

                        if (person.Person.ThumbnailPicture != null)
                        {
                            zip.AddEntry(string.Format("{0}.{1}-thumbnail.{2}", person.Person.LastName.Trim(), person.Person.FirstName.Trim(), ExtractExtension(person.Person.ContentType)), person.Person.ThumbnailPicture);    
                        }
                    }
                }

                zip.Save(stream);

                return File(stream.ToArray(), "application/zip", "agribusiness-export.zip");
            }

            return File(new byte[0], string.Empty);
        }

        private string ExtractExtension(string contentType)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(contentType))
                {
                    return contentType.Substring(contentType.IndexOf('/') + 1);
                }
            }
            catch { }

            // default to jpg
            return "jpg";
        }
    }

}
