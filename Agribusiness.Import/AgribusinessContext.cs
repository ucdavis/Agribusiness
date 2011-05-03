using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Agribusiness.Import.Models;

namespace Agribusiness.Import
{
    public class AgribusinessContext : DbContext
    {
        public DbSet<Firm> Firms { get; set; }
        public DbSet<Tracking> Trackings { get; set; }
        public DbSet<ContactFirms> ContactFirms { get; set; }
        public DbSet<Seminar> Seminars { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
    }
}