using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agribusiness.Import.Models
{
    public class Firm
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public bool? IsPOBox { get; set; }

        [DataType(DataType.Url)]
        public string Fax { get; set; }
        [DataType(DataType.Url)]
        public string Phone { get; set; }
        public string Ext { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }

        public string Description { get; set; }
        public bool? Financial { get; set; }

        public int? f_id { get; set; }

        public string Name{ get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        [DataType(DataType.Url)]
        public string WebAddress { get; set; }
    }
}