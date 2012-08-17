using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agribusiness.Core.Domain;

namespace Agribusiness.Web.Services
{
    public interface INotificationService
    {
        /// <summary>
        /// Generate the notification while filling in the token fields
        /// </summary>
        /// <param name="template"></param>
        /// <param name="person">Person that the notification is being sent to</param>
        /// <param name="seminarId">Seminar associated with message when available</param>
        /// <returns></returns>
        string GenerateNotification(string template, Person person, string siteId, int? seminarId = null, Invitation invitation = null, string password = null);

        /// <summary>
        /// Sends the notification to the admin about an information request submission.
        /// </summary>
        /// <param name="informationRequest"></param>
        void SendInformationRequestNotification(InformationRequest informationRequest);

        /// <summary>
        /// Queue the confirmation message to the person requesting information
        /// </summary>
        /// <param name="email"></param>
        void SendInformationRequestConfirmatinon(string email);

        /// <summary>
        /// Add person to the mailing list
        /// </summary>
        /// <param name="seminar"></param>
        /// <param name="person"></param>
        /// <param name="mailingListName"></param>
        void AddToMailingList(Seminar seminar, Person person, string mailingListName);

        /// <summary>
        /// Remove person from mailing list
        /// </summary>
        /// <param name="seminar"></param>
        /// <param name="person"></param>
        /// <param name="mailingListName"></param>
        void RemoveFromMailingList(Seminar seminar, Person person, string mailingListName);

        /// <summary>
        /// Add to the email queue a confirmation email on application submission
        /// </summary>
        /// <returns></returns>
        void GenerateConfirmation(Application application);
    }
}
