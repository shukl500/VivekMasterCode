using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI
{
    public  class AirportDetails
    {
        public  string country;
        public  string city_iata;
        public  string iata;
        public  string country_iata;
        public  string name;
        public  string city;
        public  Location location;

    }

    public class Location
    {
        public Double lon;
        public Double lat;
    }
}
