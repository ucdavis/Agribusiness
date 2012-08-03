using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Services;
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

        /// <summary>
        /// Is this the current seminar?
        /// </summary>
        public bool IsCurrent { get; set; }

        // used to display the details of the semianr
        public IEnumerable<DisplayPerson> DisplayPeople { get; set; }

        public static SeminarViewModel Create(IRepository repository, Site site, Seminar seminar = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new SeminarViewModel {Seminar = seminar ?? new Seminar() {Site = site}};

            return viewModel;
        }

        //public void PopulateDisplayPeople(IFirmService firmService)
        //{
        //    var firms = firmService.GetAllFirms();

        //    DisplayPeople = new List<DisplayPerson>();

        //    foreach (var a in Seminar.SeminarPeople)
        //    {
        //        var dp = new DisplayPerson() {Firm = firms.Where(b=>b.FirmCode == a.FirmCode).SingleOrDefault(),Person = a.Person, Title = a.Title};
        //        DisplayPeople.Add(dp);
        //    }

        //}
    }
}