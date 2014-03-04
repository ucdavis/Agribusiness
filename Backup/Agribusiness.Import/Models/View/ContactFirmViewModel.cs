using System.Collections.Generic;

namespace Agribusiness.Import.Models.View
{
    public class ContactFirmViewModel : ViewModelBase
    {
        public IList<ContactFirms> ContactFirms { get; set; }
        
        public static ContactFirmViewModel Create(IList<ContactFirms> contactFirms, IList<KeyValuePair<string, string>> errors, bool alreadyImported)
        {
            var viewModel = new ContactFirmViewModel(){ContactFirms = contactFirms, Errors = errors, AlreadyImported = alreadyImported};

            return viewModel;
        }
    }
}