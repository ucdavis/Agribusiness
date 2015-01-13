using System;
using System.Configuration;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Routing;

namespace Agribusiness.Web.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class IpFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            if (ConfigurationManager.AppSettings["SyncCall"] == "true")
            {
                var client = new SmtpClient();
                var msg = "Sync call from {0}";
                var message = new MailMessage("automatedemail@caes.ucdavis.edu", "jsylvestre@ucdavis.edu", "Agribusiness Sync Call IP Filter", string.Format(msg, filterContext.RequestContext.HttpContext.Request.UserHostAddress));

                client.Send(message);
            }

            // redirect if the ip doesn't match what our server is
            if (filterContext.RequestContext.HttpContext.Request.UserHostAddress != ConfigurationManager.AppSettings["IpFilter"])
            {
                //#if DEBUG
                //                try
                //                {
                //                    var client = new SmtpClient();
                //                    var msg = "Sync call from {0} was called and filtered out.";
                //                    var message = new MailMessage("automatedemail@caes.ucdavis.edu", "jsylvestre@ucdavis.edu", "Sync Call IP Filter Failure", string.Format(msg, filterContext.RequestContext.HttpContext.Request.UserHostAddress));

                //                    client.Send(message);
                //                }
                //                catch (Exception)
                //                {
                //                }
                //#endif
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "error", action = "notauthorized" }));
            }
        }
    }
}