using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Randyfacts.Data;
using Randyfacts.Response;

namespace Randyfacts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListController : ControllerBase
    {
        // GET: api/List
        [HttpGet]
        public ListResponse Get()
        {
            Store store = new Store();

            var results = store.GetFacts().Select(f => f.Content).ToArray();

            return new ListResponse
            {
                Total = results.Count(),
                Results = results
            };
        }

        // GET: api/List/5
        [HttpGet("{token}")]
        public ListResponse Get(string token)
        {
            Store store = new Store();

            var results = store.List(token).Select(f => f.Content).ToArray();

            return new ListResponse
            {
                Total = results.Count(),
                Results = results
            };
        }
    }
}
