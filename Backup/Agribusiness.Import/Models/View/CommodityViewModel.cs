using System.Collections.Generic;

namespace Agribusiness.Import.Models.View
{
    public class CommodityViewModel : ViewModelBase
    {
        public List<CommodityLink> Commodities { get; set; }
        
        public static CommodityViewModel Create(List<CommodityLink> commodities, List<KeyValuePair<string, string>> errors, bool isAlreadyImported)
        {
            var viewModel = new CommodityViewModel(){Commodities = commodities, Errors = errors, AlreadyImported = isAlreadyImported};

            return viewModel;
        }
    }
}