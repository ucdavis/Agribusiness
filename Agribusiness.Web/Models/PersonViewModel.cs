using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Person class
    /// </summary>
    public class PersonViewModel
    {
        public IList<Address> Addresses { get; set; }
        public IEnumerable<State> States { get; set; }
        public Person Person { get; set; }
        public string Email { get; set; }

        public SeminarPerson SeminarPerson { get; set; }

        public static PersonViewModel Create(IRepository repository, Person person = null, string email = null)
        {
            Check.Require(repository != null, "Repository must be supplied");

            var viewModel = new PersonViewModel()
            {
                Person = person ?? new Person(),
                Addresses = repository.OfType<AddressType>().Queryable.Select(a => new Address() { AddressType = a}).ToList(),
                States = repository.OfType<State>().GetAll(),
                SeminarPerson = person != null ? person.GetLatestRegistration() : null,
                Email = email
            };

            // find any addresses and replace them into the list
            if (person != null)
            {
                foreach(var a in person.Addresses)
                {
                    var addr = viewModel.Addresses.Where(b => b.AddressType == a.AddressType).FirstOrDefault();

                    if (addr != null) viewModel.Addresses.Remove(addr);

                    viewModel.Addresses.Add(a);
                }
            }

            // reorder so always in the same order
            viewModel.Addresses = viewModel.Addresses.OrderBy(a => a.AddressType.Id).ToList();

            return viewModel;
        }
    }

    /// <summary>
    /// Viewmodel for editing a person
    /// </summary>
    public class AdminPersonViewModel
    {
        public PersonViewModel PersonViewModel { get; set; }

        public IQueryable<SeminarRole> SeminarRoles { get; set; }
        public bool IsCurrentSeminar { get; set; }
        public int SeminarId { get; set; }

        public static AdminPersonViewModel Create(IRepository repository, ISeminarService seminarService, int seminarId, Person person = null, string email = null)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(seminarService != null, "seminarService is required.");

            var viewModel = new AdminPersonViewModel()
                                {
                                    PersonViewModel = PersonViewModel.Create(repository, person, email),
                                    SeminarRoles = repository.OfType<SeminarRole>().Queryable,
                                    SeminarId = seminarId
                                };

            // determine if last reg is the current seminar
            viewModel.IsCurrentSeminar = viewModel.PersonViewModel.SeminarPerson.Seminar == seminarService.GetCurrent();

            return viewModel;
        }
    }
}