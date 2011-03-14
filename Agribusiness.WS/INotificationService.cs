using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agribusiness.WS
{
    public interface INotificationService
    {
        void GetAllNotifications(List<string> critical, List<string> messages);
    }
}
