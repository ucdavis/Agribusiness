using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Profile membership user profile
    /// </summary>
    public class ProfileViewModel
    {
        public Firm Firm { get; set; }
        public SeminarPerson SeminarPerson { get; set; }
        public Person Person { get; set; }

        public static ProfileViewModel Create(IRepository repository, IFirmService firmService, string userId)
        {
            Check.Require(repository != null, "Repository must be supplied");
            Check.Require(firmService != null, "firmService is required.");

            var user = repository.OfType<User>().Queryable.Where(a => a.LoweredUserName == userId.ToLower()).FirstOrDefault();
		    
            Check.Require(user != null, "user is required.");

            var person = user.Person;

            Check.Require(person != null, "person is required.");

            var seminarPerson = person.GetLatestRegistration();

            var viewModel = new ProfileViewModel()
                                {
                                    Firm = seminarPerson.Firm,
                                    SeminarPerson = seminarPerson,
                                    Person = person
                                };
 
            return viewModel;
        }
    }
}