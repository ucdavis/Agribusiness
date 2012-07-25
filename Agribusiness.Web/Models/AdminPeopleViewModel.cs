using System.Collections.Generic;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Core.Repositories;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// View model for page to view all people in database
    /// </summary>
    /// <remarks>
    /// Added to program 7/25/2012
    /// </remarks>
    public class AdminPeopleViewModel
    {
        public IQueryable<Person> People { get; set; }
        public IEnumerable<Seminar> Seminars { get; set; }
        public Site Site { get; set; }
        public Seminar Seminar { get; set; }

        public static AdminPeopleViewModel Create(IRepositoryFactory repositoryFactory, Site site, int? seminarId)
        {
            var viewModel = new AdminPeopleViewModel()
                                {
                                    Site = site,
                                    Seminars = repositoryFactory.SeminarRepository.Queryable.Where(a => a.Site == site).ToList()
                                };

            if (seminarId.HasValue)
            {
                viewModel.People = repositoryFactory.PersonRepository.Queryable.Where(a => a.SeminarPeople.Select(b => b.Seminar.Id).Contains(seminarId.Value));
            }
            else
            {
                viewModel.People = repositoryFactory.PersonRepository.Queryable;
            }
            
            return viewModel;
        }

    }
}