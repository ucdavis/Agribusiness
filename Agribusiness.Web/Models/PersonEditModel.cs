using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Models
{
    public class PersonEditModel
    {
        public Person Person { get; set; }
        public IList<Address> Addresses { get; set; }
        public string Email { get; set; }

        public PersonEditModel()
        {
            Addresses = new List<Address>();
        }
    }
}