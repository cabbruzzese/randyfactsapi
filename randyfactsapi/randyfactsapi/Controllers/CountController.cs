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
    public class CountController : ControllerBase
    {
        // GET: api/Count
        [HttpGet]
        public CountResponse Get()
        {
            Store store = new Store();

            int count = store.HowMany();

            return new CountResponse
            {
                Token = null,
                Total = count
            };
        }

        // GET: api/List/searchterm
        [HttpGet("{token}")]
        public CountResponse Get(string token)
        {
            Store store = new Store();

            int count = store.HowMany(token);

            return new CountResponse
            {
                Token = null,
                Total = count
            };
        }
    }
}
