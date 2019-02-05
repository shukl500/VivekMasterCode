using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWEBAPI
{
    public class MovieClass
    {
        public string Title { get; set; }
        public string Year { get; set; }
        public string imdbID { get; set; }
        public string Poster { get; set; }
        public Snippet snippet = new Snippet();
        public string videoId { get; set; }
    }
}
