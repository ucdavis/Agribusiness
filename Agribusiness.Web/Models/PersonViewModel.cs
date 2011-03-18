using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
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

        public static PersonViewModel Create(IRepository repository, Person person = null)
        {
            Check.Require(repository != null, "Repository must be supplied");

            var viewModel = new PersonViewModel()
            {
                Person = person ?? new Person()
                ,
                Addresses = repository.OfType<AddressType>().Queryable.Select(a => new Address() { AddressType = a}).ToList(),
                States = repository.OfType<State>().GetAll()
            };
 
            return viewModel;
        }
    }
}