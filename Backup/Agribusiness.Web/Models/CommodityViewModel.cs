using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Commodity class
    /// </summary>
    public class CommodityViewModel
    {
        public virtual Commodity Commodity { get; set; }
 
        public static CommodityViewModel Create(IRepository repository, Commodity commodity = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new CommodityViewModel {Commodity = commodity ?? new Commodity()};
 
            return viewModel;
        }
    }
}