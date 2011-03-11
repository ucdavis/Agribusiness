using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
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

        public string GenerateCoupon(int itemId, Person person, SeminarRole role)
        {
            // calculate the discount amt

            // hard code in the rules

            // execute the request

            // return the coupon code

            throw new NotImplementedException();
        }

        public void CancelCoupon(int itemId, string couponCode)
        {
            throw new NotImplementedException();
        }
    }
}
