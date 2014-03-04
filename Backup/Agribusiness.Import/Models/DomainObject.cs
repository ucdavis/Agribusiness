using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agribusiness.Import.Models
{
    public class DomainObject
    {
        public DomainObject()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}