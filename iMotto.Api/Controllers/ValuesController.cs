using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace iMotto.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Hello", "iMotto" };
        }      
    }
}
