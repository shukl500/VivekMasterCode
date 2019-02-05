using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWEBAPI
{
    public class ResponseClass
    {
        public List<MovieClass> Search = new List<MovieClass>();
        public int totalResults { get; set; }
        public string Response { get; set; }
    }
}
