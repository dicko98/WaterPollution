using Cassandra;
using Cassandra.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterPollution.Domain;

namespace WaterPollution.Infrastructure
{
    [Route("[controller]")]
    [ApiController]
    public class AlertPollutionController : ControllerBase
    {
        private ICluster _cluster;
        private Cassandra.ISession _session;
        private IMapper _mapper;

        public AlertPollutionController()
        {
            _cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            _session = _cluster.Connect("WaterPollution");
            _mapper = new Mapper(_session);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var listOfPollution = _session.Execute("select * from alertPollution where about='Need to be solved' ALLOW FILTERING");
            IList<AlertPollution> pollutions = new List<AlertPollution>();
            foreach (var res in listOfPollution)
            {
                AlertPollution p = new AlertPollution();
                p.SensorId = res.GetValue<string>("sensorid");
                p.CollectTime = res.GetValue<DateTimeOffset>("collecttime");
                p.SensorData = res.GetValue<float>("sensordata");
                p.About = res.GetValue<string>("about");
                pollutions.Add(p);
            }
            return Ok(pollutions);
        }

        [HttpPost]
        public async Task Post([FromBody]PollutionDTO pollution)
        {

            string about = "Need to be solved";
            _session.Execute("INSERT INTO alertPollution (sensorid, sensordata, collecttime, about) VALUES ('" + pollution.SensorId + "', " + pollution.SensorData + ", " + pollution.CollectTime.ToUnixTimeMilliseconds() + ", 'Need to be solved')");
        }

        [HttpPut]
        public async Task Solve([FromBody] AlertPollution alertPollution)
        {
            _session.Execute("UPDATE alertPollution SET about='Solved' where sensorid='" + alertPollution.SensorId + "' AND collecttime=" + alertPollution.CollectTime.ToUnixTimeMilliseconds());
        }
    }
}
