using System.Linq;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class AdminHomeViewModel
    {
        public int PendingApplications { get; set; }
        public int PeopleMissingPicture { get; set; }
        public int FirmsRequiringReview { get; set; }

        public static AdminHomeViewModel Create(IRepository repository)
        {
            Check.Require(repository != null, "Repository is required.");



            var viewModel = new AdminHomeViewModel()
                                {
                                    PendingApplications = repository.OfType<Application>().Queryable.Where(a=>a.IsPending).Count(),
                                    PeopleMissingPicture = repository.OfType<Person>().Queryable.Where(a=>a.OriginalPicture == null).Count(),
                                    FirmsRequiringReview = repository.OfType<Firm>().Queryable.Where(a=>a.Review).Count()
                                };

            return viewModel;
        }
    }
}