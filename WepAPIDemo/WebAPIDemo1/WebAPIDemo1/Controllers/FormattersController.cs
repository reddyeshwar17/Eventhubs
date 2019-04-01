using System.Collections.Generic;
using System.Web.Http;

namespace WebAPIDemo1.Controllers
{
    public class FormattersController : ApiController
    {
        // GET api/<controller>
        [Route("api/test")]
        public IEnumerable<string> Get()
        {
            List<string> formatter = new List<string>();

            foreach (var item in GlobalConfiguration.Configuration.Formatters)
            {
                formatter.Add(item.ToString());
            }
            return formatter;
        }
    }
}