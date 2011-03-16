using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Seminar class
    /// </summary>
    public class SeminarViewModel
    {
        public Seminar Seminar { get; set; }
 
        public static SeminarViewModel Create(IRepository repository, Seminar seminar = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new SeminarViewModel {Seminar = seminar ?? new Seminar()};

            return viewModel;
        }
    }
}