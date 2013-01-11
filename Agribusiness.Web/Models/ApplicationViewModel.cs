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
        public int PersonId { get; set; }
        public bool SeminarTerms { get; set; }

        public IEnumerable<CommunicationOption> CommunicationOptions { get; set; }
        public IEnumerable<FirmType> FirmTypes { get; set; }
        
        public static ApplicationViewModel Create(IRepository repository, IFirmService firmService, string userId, string siteId, Application application = null, bool seminarTerms = false)
        {
            Check.Require(repository != null, "Repository must be supplied");

            var viewModel = new ApplicationViewModel
                                {
                                    Application = application ?? new Application(),
                                    // always get the latest
                                    Seminar = SiteService.GetLatestSeminar(siteId),
                                    //Commodities = repository.OfType<Commodity>().Queryable.Where(a=>a.IsActive).OrderBy(a=>a.Name).ToList(),
                                    Countries = repository.OfType<Country>().GetAll(),
                                    CommunicationOptions = repository.OfType<CommunicationOption>().GetAll(),
                                    SeminarTerms = seminarTerms
                                };

            // load commodities
            var commodities = repository.OfType<Commodity>().Queryable.Where(a => a.IsActive).OrderBy(a => a.Name).ToList();
            //commodities.Add(new Commodity(){ Name = "Other"});
            viewModel.Commodities = commodities;

            // load the firm types
            var firmTypes = repository.OfType<FirmType>().Queryable.Where(a => a.IsActive).OrderBy(a => a.Name).ToList();
            viewModel.FirmTypes = firmTypes;

            var user = repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == userId.ToLower()).FirstOrDefault();
            if (user == null) throw new ArgumentException(string.Format("Unable to load user with userid {0}.", userId));

            // populate the application with person info
            var person = user.Person;
            // if person is not null, there should be at least one registration (seminar person)
            if (person != null)
            {
                viewModel.PersonId = person.Id;

                viewModel.Application.FirstName = person.FirstName;
                viewModel.Application.MI = person.MI;
                viewModel.Application.LastName = person.LastName;
                viewModel.Application.BadgeName = person.BadgeName;

                viewModel.Application.CommunicationOption = person.CommunicationOption;
                viewModel.Application.ContactInformationRelease = person.ContactInformationRelease;

                // get latest seminar information
                var reg = person.GetLatestRegistration(siteId);
                if (reg != null)
                {
                    viewModel.Application.JobTitle = reg.Title;
                }

                // copy assistant information
                var assistant = person.Contacts.Where(a => a.ContactType.Id == 'A').FirstOrDefault();

                if (assistant != null)
                {
                    viewModel.Application.AssistantEmail = assistant.Email;
                    viewModel.Application.AssistantFirstName = assistant.FirstName;
                    viewModel.Application.AssistantLastName = assistant.LastName;
                    viewModel.Application.AssistantPhone = assistant.Phone;
                }

                var seminarPeople = person.GetLatestRegistration(siteId);
                if (seminarPeople != null)
                {
                    viewModel.Application.Firm = seminarPeople.Firm;    
                }

                viewModel.Application.FirmPhone = person.Phone;
                viewModel.Application.FirmPhoneExt = person.PhoneExt;

                var address = person.Addresses.Where(a => a.AddressType.Id == 'B').FirstOrDefault();
                if (address != null)
                {
                    viewModel.Application.FirmAddressLine1 = address.Line1;
                    viewModel.Application.FirmAddressLine2 = address.Line2;
                    viewModel.Application.FirmCity = address.City;
                    viewModel.Application.FirmState = address.State;
                    viewModel.Application.FirmZip = address.Zip;
                    viewModel.Application.Country = address.Country;
                }
            }

            viewModel.HasPhoto = user.Person != null && user.Person.MainProfilePicture != null;

            // get the firms and add the "Other" option
            var firms = new List<Firm>(firmService.GetAllFirms());
            viewModel.Firms = firms.Where(a=>!a.Review).OrderBy(a=>a.Name).ToList();

            return viewModel;
        }
    }
}