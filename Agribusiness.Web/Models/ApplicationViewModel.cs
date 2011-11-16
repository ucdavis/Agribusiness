using System;
using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the SeminarApplication class
    /// </summary>
    public class ApplicationViewModel
    {
        public Application Application { get; set; }
        public Seminar Seminar { get; set; }
        public IEnumerable<Commodity> Commodities { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IList<Firm> Firms { get; set; }
        public bool HasPhoto { get; set; }

        public IEnumerable<CommunicationOption> CommunicationOptions { get; set; }

        public static ApplicationViewModel Create(IRepository repository, IFirmService firmService, ISeminarService seminarService, string userId, Application application = null)
        {
            Check.Require(repository != null, "Repository must be supplied");

            var viewModel = new ApplicationViewModel
                                {
                                    Application = application ?? new Application(),
                                    // always get the latest
                                    Seminar = seminarService.GetCurrent(),
                                    Commodities = repository.OfType<Commodity>().Queryable.OrderBy(a=>a.Name).ToList(),
                                    Countries = repository.OfType<Country>().GetAll(),
                                    CommunicationOptions = repository.OfType<CommunicationOption>().GetAll()
                                };

            var user = repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == userId.ToLower()).FirstOrDefault();
            if (user == null) throw new ArgumentException(string.Format("Unable to load user with userid {0}.", userId));

            // populate the application with person info
            var person = user.Person;
            // if person is not null, there should be at least one registration (seminar person)
            if (person != null)
            {
                viewModel.Application.FirstName = person.FirstName;
                viewModel.Application.MI = person.MI;
                viewModel.Application.LastName = person.LastName;
                viewModel.Application.BadgeName = person.BadgeName;

                viewModel.Application.CommunicationOption = person.CommunicationOption;
                viewModel.Application.ContactInformationRelease = person.ContactInformationRelease;

                // copy assistant information
                var assistant = person.Contacts.Where(a => a.ContactType.Id == 'A').FirstOrDefault();

                if (assistant != null)
                {
                    viewModel.Application.AssistantEmail = assistant.Email;
                    viewModel.Application.AssistantFirstName = assistant.FirstName;
                    viewModel.Application.AssistantLastName = assistant.LastName;
                }

                var seminarPeople = person.GetLatestRegistration();
                if (seminarPeople != null)
                {
                    viewModel.Application.Firm = seminarPeople.Firm;    
                }
            }

            viewModel.HasPhoto = user.Person != null && user.Person.MainProfilePicture != null;

            // get the firms and add the "Other" option
            var firms = new List<Firm>(firmService.GetAllFirms());
            viewModel.Firms = firms.OrderBy(a=>a.Name).ToList();
            viewModel.Firms.Add(new Firm() { Name = "Other (Not Listed)" });

            return viewModel;
        }
    }
}