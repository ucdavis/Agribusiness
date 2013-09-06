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
                    seminarPerson.Seminar.RegistrationId,           //0
                    seminarPerson.ReferenceId,                      //1
                    seminarPerson.Seminar.RegistrationPassword,     //2
                    seminarPerson.CouponCode,                       //3
                    seminarPerson.Person.FirstName,                 //4
                    seminarPerson.Person.LastName,                  //5
                    seminarPerson.Title,                            //6 new
                    address.Line1,                                  //7
                    address.Line2,                                  //8
                    address.City,                                   //9
                    address.State,                                  //10
                    address.Zip,                                    //11
                    seminarPerson.Person.Phone,                     //12
                    seminarPerson.Person.User.Email);               //13
            }
            else
            {
                viewModel.CrpLink = string.Format(ConfigurationManager.AppSettings["CrpLink"],
                    seminarPerson.Seminar.RegistrationId,       //0
                    seminarPerson.ReferenceId,                  //1
                    seminarPerson.Seminar.RegistrationPassword, //2
                    seminarPerson.Person.FirstName,             //3
                    seminarPerson.Person.LastName,              //4
                    seminarPerson.Title,                        //5 new
                    address.Line1,                              //6
                    address.Line2,                              //7
                    address.City,                               //8
                    address.State,                              //9
                    address.Zip,                                //10
                    seminarPerson.Person.Phone,                 //11
                    seminarPerson.Person.User.Email);           //12
            }

            return viewModel;
        }
    }
}