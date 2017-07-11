using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace poster.Controllers
{
    [Route("api/[controller]")]
    public class SensorController : Controller
    {
        private static IEnumerable<SensorInfo> _sensorInfos = new List<SensorInfo>() {
            new SensorInfo() {Name = "My Test", Status = "Failed" }};

        // GET api/values
        [HttpGet]
        public IEnumerable<SensorInfo> Get()
        {
            return _sensorInfos;
        }

        [HttpPost]
        public void Post([FromBody]IEnumerable<SensorInfo> sensorInfos)
        {
            _sensorInfos = sensorInfos;
        }
    }

    public class SensorInfo
    {
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
