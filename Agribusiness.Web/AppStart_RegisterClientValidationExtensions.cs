using DataAnnotationsExtensions.ClientValidation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Agribusiness.Web.AppStart_RegisterClientValidationExtensions), "Start", callAfterGlobalAppStart: true)]
 
namespace Agribusiness.Web {
    public static class AppStart_RegisterClientValidationExtensions {
        public static void Start() {
            DataAnnotationsModelValidatorProviderExtensions.RegisterValidationExtensions();            
        }
    }
}