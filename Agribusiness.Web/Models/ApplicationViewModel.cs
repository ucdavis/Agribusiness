﻿using System;
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
        public IEnumerable<Firm> Firms { get; set; }
        public IEnumerable<State> States { get; set; }
        public bool HasPhoto { get; set; }

        public static ApplicationViewModel Create(IRepository repository, IFirmService firmService, string userId, Application application = null)
        {
            Check.Require(repository != null, "Repository must be supplied");

            var viewModel = new ApplicationViewModel
                                {
                                    Application = application ?? new Application(),
                                    // always get the latest
                                    Seminar = repository.OfType<Seminar>().GetNullableById(repository.OfType<Seminar>().Queryable.Max(a=>a.Id)),
                                    Commodities = repository.OfType<Commodity>().Queryable.OrderBy(a=>a.Name).ToList(),
                                    States = repository.OfType<State>().GetAll()
                                };

            var user = repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == userId.ToLower()).FirstOrDefault();
            if (user == null) throw new ArgumentException(string.Format("Unable to load user with userid {0}.", userId));

            // populate the application with person info
            var person = user.Person;
            // if person is not null, there should be at least one registration (seminar person)
            if (person != null)
            {
                var seminarPeople = person.GetLatestRegistration();

                application.FirstName = person.FirstName;
                application.MI = person.MI;
                application.LastName = person.LastName;
                application.BadgeName = person.BadgeName;

                application.Firm = firmService.GetFirm(seminarPeople.FirmCode);
            }

            viewModel.HasPhoto = user.Person != null && user.Person.MainProfilePicture != null;

            // get the firms and add the "Other" option
            var firms = new List<Firm>(firmService.GetAllFirms());
            firms.Add(new Firm(){Name="Other (Not Listed)"});

            viewModel.Firms = firms;

            return viewModel;
        }
    }
}