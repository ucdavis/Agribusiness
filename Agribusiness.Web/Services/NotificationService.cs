using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<Seminar> _seminarRepository;
        private readonly ISeminarService _seminarService;

        private Seminar _seminar;

        public NotificationService(IRepository<Seminar> seminarRepository,ISeminarService seminarService)
        {
            _seminarRepository = seminarRepository;
            _seminarService = seminarService;
        }

        public string GenerateNotification(string template, Person person, int? seminarId = null)
        {
            if (seminarId.HasValue)
            {
                _seminar = _seminarRepository.GetNullableById(seminarId.Value);
            }
            else
            {
                _seminar = _seminarService.GetCurrent();
            }

            return HandleBody(template, person);
        }

        /// <summary>
        /// Takes the template text from the database and converts it to the finalized text
        /// </summary>
        /// <param name="body">Template text from the db</param>
        /// <returns>Completed text ready for exporting</returns>
        private string HandleBody(string body, Person person)
        {
            Check.Require(_seminar != null, "_seminar is required.");
            Check.Require(person != null, "person is required.");

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

                tempbody = tempbody + replaceParameter(parameter, person);

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
        private string replaceParameter(string parameter, Person person)
        {
            // Trim the {}
            int length = parameter.Length;
            parameter = parameter.Substring(1, length - 2);

            // replace the value
            switch(parameter.ToLower())
            {
                case"badgename":
                    return string.IsNullOrWhiteSpace(person.BadgeName) ? person.FirstName : person.BadgeName;
                case "firstname":
                    return person.FirstName;
                case "fullname":
                    return person.FullName;
                case "seminarbegindate":
                    return _seminar.Begin.ToString("g");
                case "seminarenddate":
                    return _seminar.End.ToString("g");
            }

            throw new ArgumentException("Invalid parameter was passed.");
        }
    }
}