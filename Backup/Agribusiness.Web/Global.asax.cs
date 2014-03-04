using System.Web;
using System.Web.Mvc;
using Agribusiness.Core;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Controllers;
using Agribusiness.Web.Helpers;
using Castle.Windsor;
using Elmah;
using Microsoft.Practices.ServiceLocation;
using UCDArch.Data.NHibernate;
using UCDArch.Web.IoC;
using UCDArch.Web.ModelBinder;

namespace Agribusiness.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
#if DEBUG
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
#endif

            RouteConfigurator.RegisterRoutes();

            ModelBinders.Binders.DefaultBinder = new UCDArchModelBinder();

            NHibernateSessionConfiguration.Mappings.UseFluentMappings(typeof(AddressType).Assembly);

            IWindsorContainer container = InitializeServiceLocator();

            AutomapperConfig.Configure();
        }

        private static IWindsorContainer InitializeServiceLocator()
        {
            IWindsorContainer container = new WindsorContainer();

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            container.RegisterControllers(typeof(HomeController).Assembly);
            ComponentRegistrar.AddComponentsTo(container);

            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));

            return container;
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