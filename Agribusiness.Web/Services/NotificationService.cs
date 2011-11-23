using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly ISeminarService _seminarService;
        private readonly IRepository<EmailQueue> _emailQueueRepository;
        private readonly IRepository<MailingList> _mailingListRepository;
        private AccountMembershipService _membershipService;
        

        public NotificationService(IRepository<Seminar> seminarRepository,ISeminarService seminarService, IRepository<EmailQueue> emailQueueRepository, IRepository<MailingList> mailingListRepository)
        {
            _seminarRepository = seminarRepository;
            _seminarService = seminarService;
            _emailQueueRepository = emailQueueRepository;
            _mailingListRepository = mailingListRepository;

            if (_membershipService == null) { _membershipService = new AccountMembershipService(); }
        }

        public string GenerateNotification(string template, Person person, int? seminarId = null, Invitation invitation = null, string password = null)
        {
            Seminar seminar;

            if (seminarId.HasValue)
            {
                seminar = _seminarRepository.GetNullableById(seminarId.Value);
            }
            else
            {
                seminar = _seminarService.GetCurrent();
            }

            var helper = new NotificationGeneratorHelper(person, seminar, invitation, password);

            return HandleBody(template, helper);
        }

        public void SendInformationRequestNotification(InformationRequest informationRequest)
        {
            // send the notification email to the admin
            try
            {
                var client = new SmtpClient();
                var message = new MailMessage();
                var emails = ConfigurationManager.AppSettings["NotificationUsers"].Split(';');

                message.From = new MailAddress("automatedemail@caes.ucdavis.edu", "CA&ES Automated Email");
                foreach (var email in emails)
                {
                    message.To.Add(email);
                }
                message.Subject = "Information Request Received";
                message.Body = string.Format("A new information request has been received for the following person:<br/>{0}<br/>{1}", informationRequest.Name, informationRequest.Email);
                client.Send(message);
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        public void SendInformationRequestConfirmatinon(string email)
        {
            var subject = "Information Request Received";
            var body = "Thank you for your interest in the UC Davis Agribusiness Executive Seminar. We have received your request for information regarding the seminar. We will contact you with more details soon.";

            try
            {

                var client = new SmtpClient();
                var message = new MailMessage("automatedemail@caes.ucdavis.edu", email, subject, body);

                client.Send(message);
            }
            catch (Exception ex)
            {
                // do nothing
            }
        }

        public void AddToMailingList(Seminar seminar, Person person, string mailingListName)
        {
            var mailingList = seminar.MailingLists.Where(a => a.Name == mailingListName).FirstOrDefault();

            if (mailingList != null)
            {
                mailingList.AddPerson(person);    

                _mailingListRepository.EnsurePersistent(mailingList);
            }
        }

        public void RemoveFromMailingList(Seminar seminar, Person person, string mailingListName)
        {
            var mailingList = seminar.MailingLists.Where(a => a.Name == mailingListName).FirstOrDefault();

            if (mailingList != null)
            {
                mailingList.People.Remove(person);

                _mailingListRepository.EnsurePersistent(mailingList);
            }
        }

        /// <summary>
        /// Takes the template text from the database and converts it to the finalized text
        /// </summary>
        /// <param name="body">Template text from the db</param>
        /// <returns>Completed text ready for exporting</returns>
        private string HandleBody(string body, NotificationGeneratorHelper helper)
        {
            Check.Require(helper != null, "helper is required.");

            // Begin actual processing function
            string tempbody = "";
            string parameter;

            // Find the beginning of a replacement string
            int begindex = body.IndexOf("{");
            int endindex;
            while (begindex >= 0)
            {
                // Copy the text that comes before the replacement string to temp
                tempbody = tempbody + body.Substring(0, begindex);
                // Removes the first part from the string before the {
                body = body.Substring(begindex);

                // Find the end of a replacement string
                endindex = body.IndexOf("}");

                // Pulls the text between {}
                parameter = body.Substring(0, endindex + 1);
                // removes the parameter substring
                body = body.Substring(endindex + 1);

                tempbody = tempbody + replaceParameter(parameter, helper);

                // Find the beginning of a replacement string
                begindex = body.IndexOf("{");
            }

            // Gets the remaining text from the template after the last tag
            tempbody = tempbody + body;

            return tempbody;
        }

        /// <summary>
        /// Returns the string data that should be replaced into the template text
        /// to create the final letter for the students.
        /// </summary>
        /// <param name="parameter">The parameter name.</param>
        /// <returns>Value that should replace the parameter</returns>
        private string replaceParameter(string parameter, NotificationGeneratorHelper helper)
        {
            // Trim the {}
            int length = parameter.Length;
            parameter = parameter.Substring(1, length - 2);

            // replace the value
            switch (parameter.ToLower())
            {
                case "badgename":
                    return string.IsNullOrWhiteSpace(helper.BadgeName) ? helper.FirstName : helper.BadgeName;
                case "firstname":
                    return helper.FirmName;
                case "fullname":
                    return helper.FullName;
                case "seminarbegindate":
                    return helper.SeminarBegin;
                case "seminarenddate":
                    return helper.SeminarEnd;
                case "seminardeadline":
                    return helper.SeminarDeadline;
                case "title":
                    return helper.Title;
                case "firmname":
                    return helper.FirmName;
                case "username":
                    return helper.UserName;
                case "password":

                    return helper.Password ?? _membershipService.ResetPasswordNoEmail(helper.UserName);

                    //var password = _membershipService.ResetPasswordNoEmail(helper.UserName);
                    ///return password;
            }

            throw new ArgumentException("Invalid parameter was passed.");
        }
    }

    public class NotificationGeneratorHelper
    {
        public NotificationGeneratorHelper()
        {
            
        }

        public NotificationGeneratorHelper(Person person, Seminar seminar, Invitation invitation = null, string password = null)
        {
            var reg = person.GetLatestRegistration();

            BadgeName = person.BadgeName;
            FirstName = person.FirstName;
            LastName = person.LastName;
            FullName = person.FullName;
            SeminarBegin = seminar.Begin.ToString("g");
            SeminarEnd = seminar.End.ToString("g");
            SeminarDeadline = seminar.RegistrationDeadlineString;
            Title = invitation != null ? invitation.Title : (reg != null ? reg.Title : string.Empty);
            FirmName = invitation != null ? invitation.FirmName : (reg != null && reg.Firm != null ? reg.Firm.Name : string.Empty);

            UserName = person.User.UserName;
            Password = password;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }

        public string BadgeName { get; set; }

        public string SeminarBegin { get; set; }
        public string SeminarEnd { get; set; }
        public string SeminarDeadline { get; set; }

        public string Title { get; set; }
        public string FirmName { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
    }
}