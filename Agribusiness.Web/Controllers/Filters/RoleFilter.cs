using System.Web.Mvc;

namespace Agribusiness.Web.Controllers.Filters
{
    public class UserOnlyAttribute : AuthorizeAttribute
    {
        public UserOnlyAttribute()
        {
            Roles = RoleNames.User;
        }
    }

    public static class RoleNames
    {
        public static readonly string User = "User";
    }
}