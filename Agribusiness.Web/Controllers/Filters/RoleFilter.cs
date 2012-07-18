using System.Text.RegularExpressions;
using System.Web.Mvc;
using Agribusiness.Web.Models;

namespace Agribusiness.Web.Controllers.Filters
{
    public class UserOnlyAttribute : AuthorizeAttribute
    {
        public UserOnlyAttribute()
        {
            Roles = RoleNames.User;
        }
    }

    public class MembershipUserOnlyAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var userName = httpContext.User.Identity.Name;

            var membership = new AccountMembershipService();

            var result = membership.IsValidUser(userName);

            return result;


            //if (IsValidEmail(userName))
            //{
            //    return true;
            //}

            //return false;
        }

        /// <summary>
        /// method for determining is the user provided a valid email address
        /// We use regular expressions in this check, as it is a more thorough
        /// way of checking the address provided
        /// </summary>
        /// <remarks>http://www.dreamincode.net/code/snippet1374.htm</remarks>
        /// <param name="email">email address to validate</param>
        /// <returns>true is valid, false if not valid</returns>
        private bool IsValidEmail(string email)
        {
            //regular expression pattern for valid email
            //addresses, allows for the following domains:
            //com,edu,info,gov,int,mil,net,org,biz,name,museum,coop,aero,pro,tv
            string pattern = @"^[-a-zA-Z0-9][-.a-zA-Z0-9]*@[-.a-zA-Z0-9]+(\.[-.a-zA-Z0-9]+)*\.
                                (com|edu|info|gov|int|mil|net|org|biz|name|museum|coop|aero|pro|tv|[a-zA-Z]{2})$";
            //Regular expression object
            Regex check = new Regex(pattern, RegexOptions.IgnorePatternWhitespace);
            //boolean variable to return to calling method
            bool valid = false;

            //make sure an email address was provided
            if (string.IsNullOrEmpty(email))
            {
                valid = false;
            }
            else
            {
                //use IsMatch to validate the address
                valid = check.IsMatch(email);
            }
            //return the value to the calling method
            return valid;
        }
    }

    public static class RoleNames
    {
        public static readonly string User = "User";
    }
}