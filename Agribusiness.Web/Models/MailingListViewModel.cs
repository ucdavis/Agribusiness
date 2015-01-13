using System.Collections.Generic;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;
using Agribusiness.Web.Services;
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
        public IEnumerable<Person> People { get; set; }

        public static MailingListViewModel Create(IRepositoryFactory repositoryFactory, string siteId, MailingList mailingList = null, int? seminarId = null)
        {
            Check.Require(repositoryFactory!= null, "Repository must be supplied");

            var site = SiteService.LoadSite(siteId);

            var viewModel = new MailingListViewModel
                                {
                                    MailingList = mailingList ?? new MailingList(),
                                    Seminars = site.Seminars,
                                    People = repositoryFactory.PersonRepository.GetAll(),
                                    SeminarId = seminarId
                                };
 
            return viewModel;
        }
    }
}