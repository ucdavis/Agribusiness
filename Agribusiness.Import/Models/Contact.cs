using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Agribusiness.Import.Models
{
    public class Contact : DomainObject
    {
        public string Badge { get; set; }

        public string CourierCity { get; set; }
        public string CourierCountry { get; set; }
        public string CourierState { get; set; }
        public string CourierStreet { get; set; }
        public string CourierZip { get; set; }

        public string DeliveryCity { get; set; }
        public string DeliveryCountry { get; set; }
        public string DeliveryState { get; set; }
        public string DeliveryStreet { get; set; }
        public string DeliveryZip { get; set; }

        public string HomeCity { get; set; }
        public string HomeCountry { get; set; }
        public bool? HomePreferred { get; set; }
        public string HomeState { get; set; }
        public string HomeStreet { get; set; }
        public string HomeZip { get; set; }

        public string ReportCity { get; set; }
        public string ReportCountry { get; set; }
        public string ReportFirm { get; set; }
        public string ReportState { get; set; }
        public string ReportStreet { get; set; }
        public string ReportZip { get; set; }

        public string AssistantEmail { get; set; }
        public bool? AssistantEmailPreferred { get; set; }
        public bool? AssistantEmailPreferredCc { get; set; }
        public string AssistantName { get; set; }
        public string AssistantPhone { get; set; }
        public string AssistantExt { get; set; }
        public string Backup { get; set; }

        public string Biography { get; set; }
        
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string FaxDelivery { get; set; }
        public string Phone { get; set; }
        public string PhoneDelivery { get; set; }
        public string Ext { get; set; }

        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastUpdated { get; set; }
        public DateTime? DateModified { get; set; }

        public int? CurrentSeminarYear { get; set; }
        public int? CurrentYear { get; set; }

        public string EmergencyName { get; set; }
        public string EmergencyPhone { get; set; }
        public string EmergencyExt { get; set; }

        public int? CommodityByFirmId { get; set; }
        public int? CommodityByRContactFirmId { get; set; }

        public int? c_Id { get; set; }

        public string LayoutFirmName { get; set; }
        public string MiscCopyAndPaste { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MI { get; set; }
        public string Salutation { get; set; }
        public string Notes { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public string SpecialPreferences { get; set; }
        
        public bool? CurrentYearAccepted { get; set; }
        public bool? CurrentYearInvitee { get; set; }
        public bool? HasPhoto { get; set; }
        public bool? HasPhoto2 { get; set; }

        public string LoginName { get; set; }
        public string LoginName2 { get; set; }

        public string Password { get; set; }
    }
}