using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randyfacts.Response
{
    public class ListResponse
    {
        public int Total { get; set; }
        public string[] Results { get; set; }
    }
}
