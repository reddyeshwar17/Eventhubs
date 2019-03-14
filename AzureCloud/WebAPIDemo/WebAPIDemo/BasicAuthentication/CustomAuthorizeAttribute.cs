using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using WebAPIDemo.Models;

namespace WebAPIDemo.BasicAuthentication
{
    /// <summary>
    /// custom authorize filter attribute, we will use it with api for autherization
    /// Here uses the aventure database for authentication
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";
        readonly SQLInfoDBEntities Context = new SQLInfoDBEntities();

        public string UsersConfigKey { get; set; }
        public string RolesConfigKey { get; set; }

        protected CustomPrincipal CurrentUser
        {
            get { return Thread.CurrentPrincipal as CustomPrincipal; }
            set { Thread.CurrentPrincipal = value as CustomPrincipal; }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;

                if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter) && authValue.Scheme == BasicAuthResponseHeaderValue)
                {
                    Credentials parsedCredentials = ParseAuthorizationHeader(authValue.Parameter);

                    if (parsedCredentials != null)
                    {
                        //in below queries Month user name and Day password
                        // var user = Context.Users.Where(u => u.Username == parsedCredentials.Username && u.Password == parsedCredentials.Password).FirstOrDefault();
                        var user = Context.AxMonthlyDatas.Where(u => u.Month == parsedCredentials.Username && u.Day.ToString() == parsedCredentials.Password).FirstOrDefault();
                        if (user != null)
                        {
                            string roles = ""; // dummy for below line real time use below line
                            //var roles = user.Roles.Select(m => m.RoleName).ToArray();
                            var authorizedUsers = ConfigurationManager.AppSettings[UsersConfigKey];
                            var authorizedRoles = ConfigurationManager.AppSettings[RolesConfigKey];

                            Users = String.IsNullOrEmpty(Users) ? authorizedUsers : Users;
                            Roles = String.IsNullOrEmpty(Roles) ? authorizedRoles : Roles;
                            // check below stament on tutorial link
                            //https://www.dotnettricks.com/learn/webapi/securing-aspnet-web-api-using-basic-authentication
                            CurrentUser = new CustomPrincipal(parsedCredentials.Username, roles);

                            if (!String.IsNullOrEmpty(Roles))
                            {
                                if (!CurrentUser.IsInRole(Roles))
                                {
                                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                                    return;
                                }
                            }

                            if (!String.IsNullOrEmpty(Users))
                            {
                                if (!Users.Contains(CurrentUser.UserId.ToString()))
                                {
                                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                                    return;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;

            }
        }

        private Credentials ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });

            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;

            return new Credentials() { Username = credentials[0], Password = credentials[1], };
        }
    }
    //Client credential
    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
