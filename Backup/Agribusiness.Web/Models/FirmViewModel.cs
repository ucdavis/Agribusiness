using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Firm class
    /// </summary>
    public class FirmViewModel
    {
        public Firm OrigFirm { get; set; }
        public Firm PendingFirm { get; set; }

        public static FirmViewModel Create(IRepository repository, Firm firm = null, Firm pendingFirm = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new FirmViewModel {
                OrigFirm = firm ?? new Firm(),
                PendingFirm = pendingFirm ?? (firm ?? new Firm())
            };
 
            return viewModel;
        }
    }
}