using System.Collections.Generic;

namespace Agribusiness.Import.Models.View
{
    public class FirmViewModel
    {
        public IList<Firm> Firms { get; set; }
        public IList<KeyValuePair<string, string>> Errors { get; set; }

        public static FirmViewModel Create(IList<Firm> firms, IList<KeyValuePair<string, string>> errors)
        {
            var viewModel = new FirmViewModel() {Firms = firms, Errors = errors};
            return viewModel;
        }
    }
}