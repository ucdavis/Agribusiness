using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agribusiness.Import.Models
{
    public class ContactFirms : DomainObject
    {
        public int? ContactId { get; set; }
        public int? FirmId { get; set; }
        public int? rcfId { get; set; }
    }
}