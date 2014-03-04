using System.Collections.Generic;

namespace Agribusiness.Import.Models.View
{
    public class ContactViewModel : ViewModelBase
    {
        public List<Contact> Contacts { get; set; }

        public static ContactViewModel Create(List<Contact> contacts, List<KeyValuePair<string, string>> errors, bool alreadyImported)
        {
            var viewModel = new ContactViewModel() {Contacts = contacts, Errors = errors, AlreadyImported = alreadyImported};

            return viewModel;
        }
    }
}