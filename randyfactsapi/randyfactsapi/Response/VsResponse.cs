using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Randyfacts.Response.Data;

namespace Randyfacts.Response
{
    public class VsResponse
    {
        public Contender[] Contenders { get; set; }
        public string Winner { get; set; }
    }
}
