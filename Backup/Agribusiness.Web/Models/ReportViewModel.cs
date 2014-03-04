using System.Collections.Generic;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Report class
    /// </summary>
    public class ReportViewModel
    {
        public IEnumerable<Seminar> Seminars { get; set; }
        public Seminar Seminar { get; set; }

        public static ReportViewModel Create(IRepository repository, ISeminarService seminarService)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new ReportViewModel {Seminars = repository.OfType<Seminar>().GetAll(), Seminar = seminarService.GetCurrent()};
 
            return viewModel;
        }
    }
}