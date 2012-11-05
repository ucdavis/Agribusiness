using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using Agribusiness.Core.Domain;
using Agribusiness.Web.Models;
using UCDArch.Core;
using UCDArch.Core.PersistanceSupport;

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
            // load the user and make sure they are valid
            var userName = httpContext.User.Identity.Name;
            var membership = new AccountMembershipService();
            var result = membership.IsValidUser(userName);

            if (result)
            {
                // load the site id
                var siteId = httpContext.Request.RequestContext.RouteData.Values["site"];
                var personRepository = SmartServiceLocator<IRepositoryWithTypedId<Person, string>>.GetService();
                var person = personRepository.Queryable.First(a => a.User.LoweredUserName == userName.ToLower());

                //httpContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
                

                return person.Sites.Any(a => a.Id == (string)siteId);    
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var siteId = filterContext.RouteData.Values["site"];

            if (filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "NotAuthorized", controller = "Error", area = string.Empty, site = siteId }));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }

            
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