using System.Collections.Generic;

namespace Agribusiness.Import.Models.View
{
    public class FirmViewModel : ViewModelBase
    {
        public IList<Firm> Firms { get; set; }
        
        public static FirmViewModel Create(IList<Firm> firms, IList<KeyValuePair<string, string>> errors, bool alreadyImported)
        {
            var viewModel = new FirmViewModel() {Firms = firms, Errors = errors, AlreadyImported = alreadyImported};
            return viewModel;
        }
    }
}