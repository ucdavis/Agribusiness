using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agribusiness.Import.Models
{
    public class Commodity : DomainObject
    {
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public int? FirmId { get; set; }
        public int? RContactFirmId { get; set; }
        public int? m_id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public List<string> Commodities { get; set; }
    }
}