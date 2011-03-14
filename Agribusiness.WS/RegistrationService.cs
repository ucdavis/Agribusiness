using System;
using System.Configuration;
using System.ServiceModel;
using Agribusiness.Core.Domain;
using Agribusiness.WS.CrpService;

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

        public string GenerateCoupon(int itemId, Seminar seminar, Person person, SeminarRole role)
        {
            // execute the request
            var client = InitializeClient();
            var code = client.CreateCoupon(itemId, person.User.UserName, null, role.Discount.Value, 1, 1, CouponTypes.SingleUsage);

            return code;
        }

        public void CancelCoupon(int itemId, string couponCode)
        {
            var client = InitializeClient();
            client.CancelCoupon(itemId, couponCode);
        }
    }
}
