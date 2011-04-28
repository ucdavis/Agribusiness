using System;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceModel;
using Agribusiness.Core.Domain;
using Agribusiness.WS.CrpService;
using System.Linq;

namespace Agribusiness.WS
{
    public class RegistrationService : IRegistrationService
    {
        private string _userName = ConfigurationManager.AppSettings["CrpUser"];
        private string _password = ConfigurationManager.AppSettings["CrpPassword"];
        private string _url = ConfigurationManager.AppSettings["CrpServiceUrl"];

        private ItemServiceClient InitializeClient()
        {
            var binding = new BasicHttpBinding()
            {
                SendTimeout = TimeSpan.FromMinutes(1)
                ,
                Security =
                {
                    Mode = BasicHttpSecurityMode.TransportWithMessageCredential,
                    Message = { ClientCredentialType = BasicHttpMessageCredentialType.UserName }
                }
            };

            var endpointAddress = new EndpointAddress(_url);

            ItemServiceClient client = new ItemServiceClient(binding, endpointAddress);

            client.ClientCredentials.UserName.UserName = _userName;
            client.ClientCredentials.UserName.Password = _password;

            return client;
        }

        public string GenerateCoupon(int itemId, string email, decimal amount)
        {
            var client = InitializeClient();
            var coupon = client.CreateCoupon(itemId, email, null, amount, 1, 1, CouponTypes.SingleUsage);

            return coupon;
        }

        public bool CancelCoupon(int itemId, string couponCode)
        {
            var client = InitializeClient();
            return client.CancelCoupon(itemId, couponCode);
        }

        public void RefreshRegistration(int itemId, string registrationId, out string transactionId, out bool paid)
        {
            var client = InitializeClient();

            var registration = client.GetRegistrationByReference(itemId, registrationId);

            if (registration != null)
            {
                transactionId = registration.TransactionNumber;
                paid = registration.Paid;

                return;
            }

            transactionId = null;
            paid = false;
        }

        public IList<RegistrationResult> RefreshAllRegistration(int itemId)
        {
            var client = InitializeClient();

            var results = client.GetRegistrations(itemId);

            return results.Select(a => new RegistrationResult(a.ReferenceId, a.TransactionNumber, a.Paid)).ToList();
        }
    }
}
