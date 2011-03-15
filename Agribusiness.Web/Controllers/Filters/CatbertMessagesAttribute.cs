using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Web.App_GlobalResources;
using Agribusiness.WS;

namespace Agribusiness.Web.Controllers.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CatbertMessagesAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var messages = new List<string>();
            var critical = new List<string>();

            // get the messages
            var notificationService = new NotificationService();
            notificationService.GetAllNotifications(critical, messages);

            if (critical.Count > 0)
            {
                // format the messages
                var criticalMessage = string.Join("</li><li>", messages);
                criticalMessage = string.Format("<ul><li>{0}</li></ul>", criticalMessage);

                // put the strings into view data
                filterContext.Controller.ViewData[StaticIndexes.Key_CatbertCritical] = criticalMessage;
            }

            if (messages.Count > 0)
            {
                // format
                var message = String.Join("</li><li>", critical);
                message = string.Format("<ul><li>{0}</li></ul>", message);

                // put in view data
                filterContext.Controller.ViewData[StaticIndexes.Key_CatbertMessage] = message;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}