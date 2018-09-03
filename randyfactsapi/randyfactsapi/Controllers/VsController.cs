using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Randyfacts.Data;
using Randyfacts.Response;
using Randyfacts.Response.Data;

namespace randyfactsapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VsController : ControllerBase
    {
        [HttpGet]
        public VsResponse Get(string token1, string token2, string type)
        {
            Store store = new Store();

            int count1 = store.HowMany(token1);
            int count2 = store.HowMany(token2);
            string winner = count1 >= count2 ? token1 : token2;
            if (count1 == count2)
                winner = "tie";

            return new VsResponse
            {
                Contenders = new Contender[]
                {
                    new Contender
                    {
                        Name = token1,
                        Count = count1
                    },
                    new Contender
                    {
                        Name = token2,
                        Count = count2
                    }
                },
                Winner = winner
            };
        }

        // GET: api/Vs/searchterm1/searchterm2
        [HttpGet("{token1}/{token2}")]
        public VsResponse Get(string token1, string token2)
        {
            Store store = new Store();

            int count1 = store.HowMany(token1);
            int count2 = store.HowMany(token2);
            string winner = count1 >= count2 ? token1 : token2;
            if (count1 == count2)
                winner = "tie";

            return new VsResponse
            {
                Contenders = new Contender[]
                {
                    new Contender
                    {
                        Name = token1,
                        Count = count1
                    },
                    new Contender
                    {
                        Name = token2,
                        Count = count2
                    }
                },
                Winner = winner
            };
        }

        // GET: api/Vs?token1=value1&token2=value2
        [HttpPost]
        public VsResponse Post(string token1, string token2)
        {
            Store store = new Store();

            int count1 = store.HowMany(token1);
            int count2 = store.HowMany(token2);
            string winner = count1 >= count2 ? token1 : token2;
            if (count1 == count2)
                winner = "tie";

            return new VsResponse
            {
                Contenders = new Contender[]
                {
                    new Contender
                    {
                        Name = token1,
                        Count = count1
                    },
                    new Contender
                    {
                        Name = token2,
                        Count = count2
                    }
                },
                Winner = winner
            };
        }
    }
}
