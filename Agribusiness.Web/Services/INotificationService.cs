using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Services
{
    public interface INotificationService
    {
        string GenerateNotification(string template, Person person, int? seminarId = null);
    }
}
