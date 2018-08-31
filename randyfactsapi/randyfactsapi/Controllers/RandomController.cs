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
    public class RandomController : ControllerBase
    {
        // GET: api/Random
        [HttpGet]
        public RandomResponse Get()
        {
            Store store = new Store();

            return new RandomResponse
            {
                Fact = store.GetRandom().Content
            };
        }

        // GET: api/Random/5
        [HttpGet("{token}")]
        public RandomResponse Get(string token)
        {
            Store store = new Store();

            var response = store.GetRandom(token);

            if (response == null)
            {
                return new RandomResponse
                {
                    Fact = null
                };
            }

            return new RandomResponse
            {
                Fact = response == null ? response.Content : null
            };
        }
    }
}
