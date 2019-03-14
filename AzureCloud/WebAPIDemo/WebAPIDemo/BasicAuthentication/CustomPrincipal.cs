using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace WebAPIDemo.BasicAuthentication
{
    /// <summary>
    /// http basic authentiction
    /// </summary>
    /// 
    //https://www.dotnettricks.com/learn/webapi/securing-aspnet-web-api-using-basic-authentication
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username, string roles)
        {
            this.Identity = new GenericIdentity(Username, roles);
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] roles { get; set; }
    }
}