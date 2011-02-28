using System.Web;
using System.Web.Mvc;
using Agribusiness.Core;
using Agribusiness.Web.Controllers;
using Castle.Windsor;
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

            NHibernateSessionConfiguration.Mappings.UseFluentMappings(typeof(Class1).Assembly);

            IWindsorContainer container = InitializeServiceLocator();
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
    }
}