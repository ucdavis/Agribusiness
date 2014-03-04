using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Attendee class
    /// </summary>
    public class AttendeeViewModel
    {
        public SeminarPerson SeminarPerson { get; set; }
 
        public static AttendeeViewModel Create(IRepository repository, SeminarPerson seminarPerson = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new AttendeeViewModel {SeminarPerson = seminarPerson ?? new SeminarPerson()};
 
            return viewModel;
        }
    }
}