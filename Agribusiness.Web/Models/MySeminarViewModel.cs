using System.Configuration;
using System.Linq;
using Agribusiness.Core.Domain;
using Agribusiness.Web.App_GlobalResources;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Core.Utils;

namespace Agribusiness.Web.Models
{
    public class MySeminarViewModel
    {
        public SeminarPerson SeminarPerson { get; set; }
        public Seminar Seminar { get; set; }

        public string CrpLink { get; set; }

        public static MySeminarViewModel Create(IRepository repository, SeminarPerson seminarPerson)
        {
            Check.Require(repository != null, "Repository is required.");

            var viewModel = new MySeminarViewModel()
                                {
                                    SeminarPerson =  seminarPerson,
                                    Seminar = seminarPerson.Seminar
                                };

            // load business address
            var address = seminarPerson.Person.Addresses.Where(a => a.AddressType.Id == StaticIndexes.Address_Business[0]).FirstOrDefault();

            address = address ?? new Address();

            // if it has a coupon code, then add it in
            if (!string.IsNullOrWhiteSpace(seminarPerson.CouponCode))
            {
                viewModel.CrpLink = string.Format(ConfigurationManager.AppSettings["crplinkCoupon"], 
                    seminarPerson.Seminar.RegistrationId, seminarPerson.ReferenceId, seminarPerson.Seminar.RegistrationPassword,
                    seminarPerson.CouponCode,
                    seminarPerson.Person.FirstName, seminarPerson.Person.LastName,
                    address.Line1, address.Line2, address.City, address.State, address.Zip,
                    seminarPerson.Person.Phone, seminarPerson.Person.User.Email);
            }
            else
            {
                viewModel.CrpLink = string.Format(ConfigurationManager.AppSettings["CrpLink"],
                    seminarPerson.Seminar.RegistrationId, seminarPerson.ReferenceId, seminarPerson.Seminar.RegistrationPassword,
                    seminarPerson.Person.FirstName, seminarPerson.Person.LastName,
                    address.Line1, address.Line2, address.City, address.State, address.Zip,
                    seminarPerson.Person.Phone, seminarPerson.Person.User.Email);
            }

            return viewModel;
        }
    }
}