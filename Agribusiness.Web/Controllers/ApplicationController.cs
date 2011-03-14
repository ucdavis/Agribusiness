using Agribusiness.Web.Controllers.Filters;
using UCDArch.Web.Attributes;
using UCDArch.Web.Controller;

namespace Agribusiness.Web.Controllers
{
    [Version]
    [CatbertMessages]
    public abstract class ApplicationController : SuperController
    {
        public string Messages {
            get { return (string)ViewData[StaticIndexes.Key_Message]; }
            set { ViewData[StaticIndexes.Key_Message] = value; }
        }
        public string ErrorMessages
        {
            get { return (string)ViewData[StaticIndexes.Key_ErrorMessage]; }
            set { ViewData[StaticIndexes.Key_ErrorMessage] = value; }
        }
    }
}
