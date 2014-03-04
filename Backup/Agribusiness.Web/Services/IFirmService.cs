using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Services
{
    public interface IFirmService
    {
        IEnumerable<Firm> GetAllFirms();
        Firm GetFirm(Guid firmCode);
    }
}
