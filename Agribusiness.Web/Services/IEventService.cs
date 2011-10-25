using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Services
{
    public interface IEventService
    {
        void Invite(Person person);
        void Apply(Person person);
        void Accepted(Person person);
        void Denied(Person person);
        void Paid(Person person);
        void HotelUpdate(Person person);
        void PhotoUpdate(Person person);
        void BioUpdate(Person person);
    }
}