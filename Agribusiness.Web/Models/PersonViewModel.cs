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
        public IList<Contact> Contacts { get; set; }
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<Commodity> Commodities { get; set; }
        public IEnumerable<Firm> Firms { get; set; }
        public Person Person { get; set; }
        public string Email { get; set; }
        public Seminar Seminar { get; set; }

        public SeminarPerson SeminarPerson { get; set; }
        public Firm Firm { get; set; }

        public static PersonViewModel Create(IRepository repository, IFirmService firmService, Seminar seminar = null, Person person = null, string email = null, Firm firm = null)
        {
            Check.Require(repository != null, "Repository must be supplied");

            var viewModel = new PersonViewModel()
            {
                Person = person ?? new Person(),
                Addresses = repository.OfType<AddressType>().Queryable.Select(a => new Address() { AddressType = a}).ToList(),
                Contacts = repository.OfType<ContactType>().Queryable.Select( a => new Contact(){ContactType = a}).ToList(),
                Countries = repository.OfType<Country>().GetAll(),
                SeminarPerson = person != null ? person.GetLatestRegistration() : null,
                Email = email,
                Seminar = seminar,
                Commodities = repository.OfType<Commodity>().Queryable.Where(a=>a.IsActive).ToList(),
                Firm = firm ?? new Firm()
            };

            if (firm == null && viewModel.SeminarPerson != null)
            {
                viewModel.Firm = viewModel.SeminarPerson.Firm;
            }

            // find any addresses and replace them into the list
            if (person != null)
            {
                foreach(var a in person.Addresses)
                {
                    var addr = viewModel.Addresses.Where(b => b.AddressType == a.AddressType).FirstOrDefault();

                    if (addr != null) viewModel.Addresses.Remove(addr);

                    viewModel.Addresses.Add(a);
                }

                foreach (var a in person.Contacts)
                {
                    var ct = viewModel.Contacts.Where(b => b.ContactType == a.ContactType).FirstOrDefault();

                    if (ct != null) viewModel.Contacts.Remove(ct);

                    viewModel.Contacts.Add(a);
                }
            }

            // reorder so always in the same order
            viewModel.Addresses = viewModel.Addresses.OrderBy(a => a.AddressType.Id).ToList();
            viewModel.Contacts = viewModel.Contacts.OrderBy(a => a.ContactType.Id).ToList();

            // get the firms and add the "Other" option
            var firms = new List<Firm>(firmService.GetAllFirms());
            firms.Add(new Firm() { Name = "Other (Not Listed)" });

            viewModel.Firms = firms;

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
        public IQueryable<RoomType> RoomTypes { get; set; }
        public bool IsCurrentSeminar { get; set; }
        public int? SeminarId { get; set; }

        public static AdminPersonViewModel Create(IRepository repository, IFirmService firmService, ISeminarService seminarService, int? seminarId, Person person = null, string email = null)
        {
            Check.Require(repository != null, "Repository is required.");
            Check.Require(seminarService != null, "seminarService is required.");

            var seminar = seminarId.HasValue ? repository.OfType<Seminar>().GetNullableById(seminarId.Value) : null;
            var viewModel = new AdminPersonViewModel()
                                {
                                    PersonViewModel = PersonViewModel.Create(repository, firmService, seminar, person, email),
                                    SeminarRoles = repository.OfType<SeminarRole>().Queryable,
                                    RoomTypes = repository.OfType<RoomType>().Queryable.Where(a=>a.IsActive),
                                    SeminarId = seminarId
                                };

            // determine if last reg is the current seminar
            if (seminar != null)
            {
                viewModel.IsCurrentSeminar = seminar == seminarService.GetCurrent();
            }
            
            //viewModel.IsCurrentSeminar = viewModel.PersonViewModel.SeminarPerson.Seminar == seminarService.GetCurrent();

            return viewModel;
        }
    }
}