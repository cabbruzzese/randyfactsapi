using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Randyfacts.Data;

namespace randyfactsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountController : ControllerBase
    {
        // GET: api/Count
        [HttpGet]
        public string Get()
        {
            Store store = new Store();

            return store.HowMany().ToString();
        }

        // GET: api/List/5
        [HttpGet("{token}")]
        public string Get(string token)
        {
            Store store = new Store();

            return store.HowMany(token).ToString();
        }
    }
}
