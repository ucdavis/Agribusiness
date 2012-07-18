using System.Web;
using Agribusiness.Web.Helpers;
using Elmah;

namespace Agribusiness.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfigurator.RegisterRoutes();

            AutomapperConfig.Configure();
        }

        void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs args)
        {
            Filter(args);
        }

        void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs args)
        {
            Filter(args);
        }

        void Filter(ExceptionFilterEventArgs args)
        {
            if (args.Exception.GetBaseException() is HttpRequestValidationException)
                args.Dismiss();

            if (args.Exception.GetBaseException().Message.Contains("does not implement IController"))
            {
                args.Dismiss();
            }

            if (args.Exception.GetBaseException().Message.Contains("Server cannot modify cookies after HTTP headers have been sent."))
            {
                args.Dismiss();
            }
            
            if (args.Exception.GetBaseException().Message.Contains("Cannot redirect after HTTP headers have been sent."))
            {
                args.Dismiss();
            }
        }
    }
}