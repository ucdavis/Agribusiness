using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers.Filters;
using Agribusiness.Web.Models;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.ActionResults;
using UCDArch.Web.Helpers;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the Sponsor class
    /// </summary>
    [UserOnly]
    public class SponsorController : ApplicationController
    {
	    private readonly IRepository<Sponsor> _sponsorRepository;

        public SponsorController(IRepository<Sponsor> sponsorRepository)
        {
            _sponsorRepository = sponsorRepository;
        }
    
        //
        // GET: /Sponsor/
        public ActionResult Index()
        {
            var sponsorList = _sponsorRepository.Queryable.Where(a => a.Site.Id == Site && a.IsActive);

            return View(sponsorList);
        }

        public ActionResult Create()
        {
            var viewModel = SponsorViewModel.Create();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Sponsor sponsor, HttpPostedFileBase logo)
        {
            ModelState.Clear();

            if (logo != null && logo.ContentLength > 0)
            {
                var ms = new MemoryStream();
                logo.InputStream.CopyTo(ms);
                sponsor.Logo = ms.ToArray();
                sponsor.LogoContentType = logo.ContentType;
            }

            sponsor.Site = SiteService.LoadSite(Site, true);
            sponsor.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                RepositoryFactory.SponsorRepository.EnsurePersistent(sponsor);
                Message = "Sponsor was successfully created";
                return RedirectToAction("Index");
            }

            var viewModel = SponsorViewModel.Create(sponsor);
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var sponsor = RepositoryFactory.SponsorRepository.GetNullableById(id);

            if (sponsor == null)
            {
                Message = "Sponsor was not found.";
                return RedirectToAction("Index");
            }

            var viewModel = SponsorViewModel.Create(sponsor);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Sponsor sponsor, HttpPostedFileBase logo)
        {
            var sponsorToEdit = RepositoryFactory.SponsorRepository.GetNullableById(id);

            if (sponsorToEdit == null)
            {
                Message = "Sponsor was not found.";
                return RedirectToAction("Index");
            }

            if (logo != null && logo.ContentLength > 0)
            {
                var ms = new MemoryStream();
                logo.InputStream.CopyTo(ms);
                sponsorToEdit.Logo = ms.ToArray();
                sponsorToEdit.LogoContentType = logo.ContentType;
            }

            AutoMapper.Mapper.Map(sponsor, sponsorToEdit);
            ModelState.Clear();
            sponsorToEdit.TransferValidationMessagesTo(ModelState);

            if (ModelState.IsValid)
            {
                RepositoryFactory.SponsorRepository.EnsurePersistent(sponsorToEdit);
                Message = "Sponsor was successfully updated";
                return RedirectToAction("Index");
            }

            var viewModel = SponsorViewModel.Create(sponsor);
            return View(viewModel);
        }

        public ActionResult Reorder()
        {
            var sponsors = RepositoryFactory.SponsorRepository.Queryable.Where(a => a.Site.Id == Site && a.IsActive);
            return View(sponsors);
        }

        [HttpPost]
        public JsonNetResult ReorderSponsors(int[] ids)
        {
            if (ids == null) return new JsonNetResult(false);

            try
            {
                var sponsors = RepositoryFactory.SponsorRepository.Queryable.Where(a => ids.Contains(a.Id)).ToList();

                for (var i = 0; i < ids.Length; i++)
                {
                    // get the sponsorid
                    var sid = ids[i];

                    // update the sponsor itself
                    var sponsor = sponsors.First(a => a.Id == sid);
                    sponsor.Order = i;

                    RepositoryFactory.SponsorRepository.EnsurePersistent(sponsor);
                }
                
                return new JsonNetResult(true);
            }
            catch 
            {
                return new JsonNetResult(false);
            }
        }

        public FileResult Logo(int id)
        {
            var sponsor = RepositoryFactory.SponsorRepository.GetNullableById(id);
            
            if (sponsor != null)
            {
                return File(sponsor.Logo, sponsor.LogoContentType);
            }

            return File(new byte[0], string.Empty);
        }
    }
}
