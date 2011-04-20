using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class FirmListViewModel
    {
        public IEnumerable<Firm> Firms { get; set; }
        public IEnumerable<Firm> PendingFirms { get; set; }

        public static FirmListViewModel Create(IRepository<Firm> firmRepository, IFirmService firmService)
        {
            Check.Require(firmRepository != null, "Repository is required.");

            var viewModel = new FirmListViewModel()
                                {
                                    Firms = firmService.GetAllFirms(),
                                    PendingFirms = firmRepository.Queryable.Where(a=>a.Review).OrderBy(a=>a.Id).ToList()
                                };

            return viewModel;
        }
    }
}