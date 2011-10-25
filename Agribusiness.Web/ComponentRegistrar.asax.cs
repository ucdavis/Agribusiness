using Agribusiness.Web.Services;
using Agribusiness.WS;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using UCDArch.Core.CommonValidator;
using UCDArch.Core.DataAnnotationsValidator.CommonValidatorAdapter;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Data.NHibernate;
using INotificationService = Agribusiness.Web.Services.INotificationService;
using NotificationService = Agribusiness.Web.Services.NotificationService;

namespace Agribusiness.Web
{
    internal static class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddGenericRepositoriesTo(container);

            container.Register(Component.For<IValidator>().ImplementedBy<Validator>().Named("validator"));
            container.Register(Component.For<IDbContext>().ImplementedBy<DbContext>().Named("dbContext"));

            container.Register(Component.For<IPictureService>().ImplementedBy<PictureService>().Named("PictureService"));
            container.Register(Component.For<IPersonService>().ImplementedBy<PersonService>().Named("PersonService"));
            container.Register(Component.For<IFirmService>().ImplementedBy<FirmService>().Named("FirmService"));
            container.Register(Component.For<ISeminarService>().ImplementedBy<SeminarService>().Named("SeminarService"));

            container.Register(Component.For<IRegistrationService>().ImplementedBy<RegistrationService>().Named("RegistrationService"));
            container.Register(Component.For<INotificationService>().ImplementedBy<NotificationService>().Named("NotificationService"));
            container.Register(Component.For<IEventService>().ImplementedBy<EventService>().Named("EventService"));
            container.Register(Component.For<IvCardService>().ImplementedBy<vCardService>().Named("vCardService"));
        }

        private static void AddGenericRepositoriesTo(IWindsorContainer container)
        {
            container.Register(Component.For(typeof(IRepositoryWithTypedId<,>)).ImplementedBy(typeof(RepositoryWithTypedId<,>)).Named("repositoryWithTypedId"));
            container.Register(Component.For(typeof(IRepository<>)).ImplementedBy(typeof(Repository<>)).Named("repositoryType"));
            container.Register(Component.For<IRepository>().ImplementedBy<Repository>().Named("repository"));
        }
    }
}