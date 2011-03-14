using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Caching;
using Agribusiness.WS.CatbertMessageService;

namespace Agribusiness.WS
{
    public class NotificationService : INotificationService
    {
        private string _url = ConfigurationManager.AppSettings["CatbertMessageServiceUrl"];

        private MessageServiceClient InitializeClient()
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

            MessageServiceClient client = new MessageServiceClient(binding, endpointAddress);

            //client.ClientCredentials.UserName.UserName = _userName;
            //client.ClientCredentials.UserName.Password = _password;

            return client;
        }

        private const string CatbertMessagesKey = "CatbertMessages";
        private const string CatbertCriticalKey = "CatbertCritical";
        private const string Agribusiness = "Agribusiness";

        public void GetAllNotifications(List<string> critical, List<string> messages)
        {
            var cache = HttpContext.Current.Cache;

            // check the cache for values
            if (HttpContext.Current.Cache[CatbertMessagesKey] != null && HttpContext.Current.Cache[CatbertCriticalKey] != null)
            {
                messages = (List<string>) cache[CatbertMessagesKey];
                critical = (List<string>) cache[CatbertCriticalKey];
            }
            else
            {
                var client = InitializeClient();

                var serviceMessages = client.GetMessages(Agribusiness);

                critical = serviceMessages.Where(a => a.Critical).Select(a => a.Message).ToList();
                messages = serviceMessages.Where(a => !a.Critical).Select(a => a.Message).ToList();

                // cache the values for 4 hours
                cache.Insert(CatbertMessagesKey, messages, null, DateTime.Now.AddHours(4), Cache.NoSlidingExpiration);
                cache.Insert(CatbertCriticalKey, critical, null, DateTime.Now.AddHours(4), Cache.NoSlidingExpiration);
            }
        }
    }
}
