using System.Collections.Generic;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Template class
    /// </summary>
    public class TemplateViewModel
    {
        public Template Template { get; set; }

        public IEnumerable<NotificationType> NotificationTypes { get; set; }

        public static TemplateViewModel Create(IRepository repository, Template template = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new TemplateViewModel
                                {
                                    Template = template ?? new Template(),
                                    NotificationTypes = repository.OfType<NotificationType>().GetAll()
                                };
 
            return viewModel;
        }
    }
}