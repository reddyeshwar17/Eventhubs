using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace WebAPIDemo.Controllers
{
    //token baesed authentication https://www.youtube.com/watch?v=rMA69bVv0U8
    public class DataController : ApiController
    {
        //for all users
        [AllowAnonymous]
        [Route("api/data/forall")]
        public IHttpActionResult Get()
        {
            return Ok("Now server time is: " + DateTime.Now.ToShortDateString());
        }

        //for all loggged in users
        // get token by providing username, pwd and grant_type, and url is http://localhost:55247/token (postman) it will genearte token, next
        //key as authurization, value as bearer (generated token value) this time url http://localhost:55247/api/data/authenticate
        [Authorize]
        [Route("api/data/authenticate")]
        public IHttpActionResult GetForAuthenticate()
        {
            var identity = (ClaimsIdentity)User.Identity;
            return Ok("Hello : " + identity.Name);
        }

        //for all admin users
        // get token by providing username, pwd and grant_type, and url is http://localhost:55247/token (postman) it will genearte token, next
        //key as authurization, value as bearer (generated token value) this time url http://localhost:55247/api/data/authenticate

        [Authorize(Roles = "admin")]
        [Route("api/data/authorize")]
        public IHttpActionResult GetForAdmin()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            return Ok("Hello :" + identity.Name + "; Roles :" + string.Join(",", roles.ToList()));
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}