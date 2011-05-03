using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Agribusiness.Import.Models
{
    public class Seminar : DomainObject
    {
        public string ApplicationStatus { get; set; }
        public string ContactName { get; set; }

        public DateTime? DateApplicationFormComplete { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public string Expectations { get; set; }
        public int? ContactId { get; set; }
        public int? s_Id { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        
        public bool? IsCaseExecutive { get; set; }
        public bool? IsDiscussionGroupLead { get; set; }
        public bool? IsFaculty { get; set; }
        public bool? IsSteeringCommittee { get; set; }
        public bool? IsPanelist { get; set; }
        public bool? IsParticipant { get; set; }
        public bool? IsSpeaker { get; set; }
        public bool? IsStaff { get; set; }
        public bool? IsVendor { get; set; }
        public bool? Accepted { get; set; }
        public bool? ExpensesComped { get; set; }

        public bool? IsApplicant { get; set; }
        public bool? IsInvitee { get; set; }

        public int? Year { get; set; }
        public int? PreviousYear { get; set; }
    }
}