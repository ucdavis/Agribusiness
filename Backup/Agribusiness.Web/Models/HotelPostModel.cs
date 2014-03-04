using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Models
{
    public class HotelPostModel
    {
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Confirmation { get; set; }
        public RoomType RoomType { get; set; }
        public string Comments { get; set; }
    }
}