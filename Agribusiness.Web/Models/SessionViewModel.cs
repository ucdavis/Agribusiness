﻿using System.Collections.Generic;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    /// <summary>
    /// ViewModel for the Session class
    /// </summary>
    public class SessionViewModel
    {
        public Session Session { get; set; }
        public IEnumerable<SessionType> SessionTypes { get; set; }
        public int SeminarId { get; set; }

        public static SessionViewModel Create(IRepository repository, int seminarId, Session session = null)
        {
            Check.Require(repository != null, "Repository must be supplied");
			
            var viewModel = new SessionViewModel
                                {
                                    Session = session ?? new Session(), 
                                    SeminarId = seminarId,
                                    SessionTypes = repository.OfType<SessionType>().GetAll()
                                };
 
            return viewModel;
        }
    }
}