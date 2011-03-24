using System.Collections.Generic;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class AssignToSessionViewModel
    {
        public Seminar Seminar { get; set; }
        
        public static AssignToSessionViewModel Create(Seminar seminar)
        {
            var viewModel = new AssignToSessionViewModel(){Seminar =  seminar};
            return viewModel;
        }
    }
}