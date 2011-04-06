using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Services
{
    public interface ISeminarService
    {
        Seminar GetCurrent();

        void CreateSeminarPerson(Application application, ModelStateDictionary modelState);
    }
}
