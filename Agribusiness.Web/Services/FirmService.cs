using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;

namespace Agribusiness.Web.Services
{
    public class FirmService : IFirmService
    {
        private readonly IRepository<Firm> _firmRepository;

        public FirmService(IRepository<Firm> firmRepository)
        {
            _firmRepository = firmRepository;
        }

        /// <summary>
        /// Gets a list of firms with the latest revision of each
        /// </summary>
        /// <remarks>
        /// eventually this would be the ideal query, until nhibernate supports it
        ///     from firm in Firms
        ///     where 
        ///     (from a in Firms
        ///     group a by a.FirmCode into b
        ///     select b.Max(c=>c.Id)
        ///     ).Contains(firm.Id)
        ///     select firm
        /// </remarks>
        /// <returns></returns>
        public IEnumerable<Firm> GetAllFirms()
        {
            // load the ids of latest revisions of each firm
            var firmIds = (from a in _firmRepository.Queryable
                           where !a.Review
                           group a by a.FirmCode into b
                           select b.Max(c => c.Id)).ToList();

            // get the firms
            var firms = _firmRepository.Queryable.Where(a => firmIds.Contains(a.Id)).ToList();

            return firms;
        }

        /// <summary>
        /// Gets the latest revision of a specific firm
        /// </summary>
        /// <param name="firmCode"></param>
        /// <returns></returns>
        public Firm GetFirm(Guid firmCode)
        {
            var firm = _firmRepository.Queryable
                        .Where(a => !a.Review && a.Id == _firmRepository.Queryable.Where(b => b.FirmCode == firmCode && !b.Review)
                                            .Select(b => b.Id).Max())
                        .FirstOrDefault();

            return firm;
        }
    }
}