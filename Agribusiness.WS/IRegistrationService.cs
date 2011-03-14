using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agribusiness.Core.Domain;

namespace Agribusiness.WS
{
    public interface IRegistrationService
    {
        string GenerateCoupon(int itemId, Seminar seminar, Person person, SeminarRole role);
        void CancelCoupon(int itemId, string couponCode);
    }
}
