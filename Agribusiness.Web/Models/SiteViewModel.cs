using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Site class
    /// </summary>
    public class SiteViewModel
    {
        public Site Site { get; set; }
 
        public static SiteViewModel Create(Site site = null)
        {
            var viewModel = new SiteViewModel {Site = site ?? new Site()};
 
            return viewModel;
        }
    }
}