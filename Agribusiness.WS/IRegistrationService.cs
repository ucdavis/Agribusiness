using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agribusiness.Core.Domain;

namespace Agribusiness.WS
{
    public interface IRegistrationService
    {
        string GenerateCoupon(int itemId, string email, decimal amount);
        bool CancelCoupon(int itemId, string couponCode);

        void RefreshRegistration(int itemId, string registrationId, out string transactionId, out bool paid);
        IList<RegistrationResult> RefreshAllRegistration(int itemId);
    }
}
