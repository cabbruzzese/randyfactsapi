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
    public class ListController : ControllerBase
    {
        // GET: api/List
        [HttpGet]
        public IEnumerable<string> Get()
        {
            Store store = new Store();

            return store.GetFacts().Select(f => f.Content);
        }

        // GET: api/List/5
        [HttpGet("{token}")]
        public IEnumerable<string> Get(string token)
        {
            Store store = new Store();

            return store.List(token).Select(f => f.Content);
        }
    }
}
