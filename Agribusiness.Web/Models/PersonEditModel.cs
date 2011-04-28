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
        public int SeminarPersonId { get; set; }
        public IList<Address> Addresses { get; set; }
        public IList<Contact> Contacts { get; set; }
        public IList<Commodity> Commodities { get; set; }
        public string Email { get; set; }
        public Firm Firm { get; set; }

        public string FirmName { get; set; }
        public string FirmDescription { get; set; }

        public PersonEditModel()
        {
            Addresses = new List<Address>();
            Contacts = new List<Contact>();
            Commodities = new List<Commodity>();
        }
    }
}