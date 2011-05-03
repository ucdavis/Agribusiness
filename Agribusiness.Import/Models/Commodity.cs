using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agribusiness.Import.Models
{
    public class CommodityLink : DomainObject
    {
        public CommodityLink()
        {
            Commodities = new List<Commodity>();
        }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int? FirmId { get; set; }
        public int? RContactFirmId { get; set; }
        public int? m_id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Commodity> Commodities { get; set; }

        public void AddCommodity(Commodity commodity)
        {
            commodity.CommodityLink = this;
            Commodities.Add(commodity);
        }
    }

    public class Commodity : DomainObject
    {
        public string Name { get; set; }
        public CommodityLink CommodityLink { get; set; }

        
    }
}