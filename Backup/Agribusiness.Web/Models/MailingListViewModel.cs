using System.Collections.Generic;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the MailingList class
    /// </summary>
    public class MailingListViewModel
    {
        public MailingList MailingList { get; set; }
        public IEnumerable<Seminar> Seminars { get; set; }
        public int? SeminarId { get; set; }

        public static MailingListViewModel Create(IRepository repository, MailingList mailingList = null, int? seminarId = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new MailingListViewModel
                                {
                                    MailingList = mailingList ?? new MailingList(),
                                    Seminars = repository.OfType<Seminar>().GetAll(),
                                    SeminarId = seminarId
                                };
 
            return viewModel;
        }
    }
}