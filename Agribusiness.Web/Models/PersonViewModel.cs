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
        public Person Person { get; set; }
 
        public static PersonViewModel Create(IRepository repository, Person person = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new PersonViewModel {Person = person ?? new Person()};
 
            return viewModel;
        }
    }
}