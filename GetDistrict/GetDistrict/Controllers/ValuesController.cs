using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;

namespace GetDistrict.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            try
            {
                //Reading the values from the XML.
                List<string> districtName = new List<string>();
                XmlDocument Data = new XmlDocument();
                Data.Load("Districts.xml");
                foreach (XmlNode singleNode in Data.DocumentElement.ChildNodes)
                {
                    districtName.Add(singleNode.InnerText);
                }

                return districtName.AsEnumerable();
            }
            catch (Exception)
            {

                return new string[] { "Error in finding the file. Please contact the adminstrator." };
            }
            
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "This funcitonality is not yet implemnted";
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
