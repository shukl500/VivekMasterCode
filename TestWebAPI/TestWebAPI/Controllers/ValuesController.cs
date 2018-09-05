using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestWebAPI.Controllers
{

    public class ValuesController : Nancy.NancyModule
    {
        AirportDetails airportDetails = new AirportDetails();
        double distance = 0.0;
        bool invalidAirpot = false;

        /// <summary>
        /// Default constructor initialization. All route has been configured in the same.
        /// </summary>
        public ValuesController()
        {
            Get("/", x=>"Default route for the web service. Please use /Calculate after the url.");
            Get("/calculate", x => "This web service is calculate the distance between two airport. The input need to be given semi colon seperated");
            Get("/calculate/{source}",x=> Index(x));

        }

        private dynamic Index(dynamic parameter)
        {
            var input = parameter.source;
            return CalculateDistanceByAPI(input);
            //return Convert.ToString(CalculateDistanceForCities(input));
        }

        /// <summary>
        /// This method will help in calculating the distance by calling the existing API for data.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CalculateDistanceByAPI(string input)
        {
            string result;
            double latitude1 = 0.0, latitude2 = 0.0, longitude1 = 0.0, longitude2 = 0.0;
            Dictionary<int, AirportDetails> list = new Dictionary<int, AirportDetails>();
            string[] cities = input.Split(";");
            int count = 1;
            foreach (string city in cities)
            {
                APICall(city);
                if(city.Equals(airportDetails.iata))
                {
                    if (count == 1)
                    {
                        latitude1 = airportDetails.location.lat;
                        longitude1 = airportDetails.location.lon;
                    }
                    else
                    {
                        latitude2 = airportDetails.location.lat;
                        longitude2 = airportDetails.location.lon;
                    }

                }
                count++;
            }
            distance = DistanceByLatitude(latitude1, latitude2, longitude1, longitude2);
            if(distance == 0 || invalidAirpot )
            {
                result = "Airport not found";
            }
            else
            {
                result= Convert.ToString(distance);
            }
            return result;
        }

        /// <summary>
        /// The method to call the existing web API given for refernce. This will help in getting all the data we need.
        /// </summary>
        /// <param name="cityITA"></param>
        /// <returns></returns>
        private  string APICall(string cityITA)
        {
            string apiUrl = "https://places-dev.cteleport.com/airports/" + cityITA;

            using (var httpClient = new HttpClient())
            {
                var response = httpClient.GetAsync(apiUrl).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode.ToString().Equals("OK"))
                    airportDetails = JsonConvert.DeserializeObject<AirportDetails>(result);
                else
                {
                    airportDetails.iata = "Not Found";
                    invalidAirpot = true;
                }

                return result;
            }

        }
        #region
        //Commmented Code
        ///// <summary>
        ///// This method is to get the input slipt and search in defined data.
        ///// </summary>
        ///// <param name="input"></param>
        //private Double CalculateDistanceForCities(string input)
        //{
        //    Dictionary<string, Dictionary<double, double>> citylocationlist = new Dictionary<string, Dictionary<double, double>>();
        //    double latitude1 = 0.0, latitude2 = 0.0, longitude1 = 0.0, longitude2 = 0.0;
        //    XmlDocument xdoc = new XmlDocument();
        //    xdoc.Load("CityDetails.xml");
        //    string[] cities = input.Split(";");
        //    int count = 1;
        //    //Loop for the spliting the two airport city
        //    foreach (string ita in cities)
        //    {
        //        XmlNodeList list = xdoc.GetElementsByTagName(ita.ToUpper());
        //        foreach (XmlNode node in list)
        //        {
        //            if (count == 1)
        //            {
        //                latitude1 = Convert.ToDouble(node.SelectSingleNode("Location").SelectSingleNode("Latitude").InnerText);
        //                longitude1 = Convert.ToDouble(node.SelectSingleNode("Location").SelectSingleNode("Longitude").InnerText);
        //            }
        //            else
        //            {
        //                latitude2 = Convert.ToDouble(node.SelectSingleNode("Location").SelectSingleNode("Latitude").InnerText);
        //                longitude2 = Convert.ToDouble(node.SelectSingleNode("Location").SelectSingleNode("Longitude").InnerText);
        //            }
        //        }
        //        count++;

        //    }
        //    return DistanceByLatitude(latitude1, latitude2, longitude1, longitude2);
        //}
        #endregion

        /// <summary>
        /// This method is specific to the calculation of distance between two location.
        /// </summary>
        /// <param name="latitude1"></param>
        /// <param name="latitude2"></param>
        /// <param name="longitude1"></param>
        /// <param name="longitude2"></param>
        /// <returns></returns>
        private double DistanceByLatitude(double latitude1, double latitude2, double longitude1, double longitude2)
        {
            var p = Math.PI / 180; // The value of Pi/180
            var a = 0.5 - Math.Cos((latitude2 - latitude1) * p) / 2 +
                     Math.Cos(latitude2 * p) * Math.Cos(latitude1 * p) *
                    (1 - Math.Cos((longitude2 - longitude1) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a));
        }

      
    }
}
