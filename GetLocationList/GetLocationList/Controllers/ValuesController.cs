using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;

namespace GetLocationList.Controllers
{
    [Route("GetListOfLocations/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Thanks for using API. This only for the fetch based on input." };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public IEnumerable<string> Get(string id)
        {
            try
            {
                List<string> ListOfLocation = new List<string>();
                XmlDocument document = new XmlDocument();
                document.Load("ListOfLocations.xml");
                XmlDocument list = new XmlDocument();
                list.LoadXml(document.GetElementsByTagName(id)[0].OuterXml);
                foreach (XmlNode singleNode in list.GetElementsByTagName("Name"))
                {
                    ListOfLocation.Add(singleNode.InnerText);
                }

                return ListOfLocation.AsEnumerable<string>();
            }
            catch (NullReferenceException )
            {
                return  new string[] { "The value supplied is present in our Records."};   
            }
            
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
