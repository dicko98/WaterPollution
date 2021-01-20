using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Cassandra.Mapping;
using WaterPollution.Domain;

namespace WaterPollution.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PollutionController : ControllerBase
    {
        private ICluster _cluster;
        private Cassandra.ISession _session;
        private IMapper _mapper;
        public PollutionController()
        {
            _cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
            _session = _cluster.Connect("WaterPollution");
            _mapper = new Mapper(_session);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {           
            IList<PollutionDTO> pollutions = new List<PollutionDTO>();
            var listOfP = _mapper.Fetch<PollutionDTO>();
            foreach(var r in listOfP)                     
                pollutions.Add(r);       
            return Ok(pollutions);
        }
        [HttpGet(nameof(GetByParams))]
        public async Task<ActionResult> GetByParams(string sensorId, DateTimeOffset from, DateTimeOffset to)
        {
            var listOfPollution = _session.Execute("SELECT * FROM pollution_data WHERE sensorid='" + sensorId + "' AND collecttime>" + from.ToUnixTimeMilliseconds() + " AND collecttime<" + to.ToUnixTimeMilliseconds() + "");
            IList<PollutionDTO> pollutions = new List<PollutionDTO>();
            foreach (var res in listOfPollution)
            {
                PollutionDTO p = new PollutionDTO();
                p.SensorId = res.GetValue<string>("sensorid");
                p.CollectTime = res.GetValue<DateTimeOffset>("collecttime");
                p.SensorData = res.GetValue<float>("sensordata");
                pollutions.Add(p);
            }
            return Ok(pollutions);
        }

        [HttpGet(nameof(GetSensors))]
        public async Task<ActionResult> GetSensors()
        {
            var listOfSensors = _session.Execute("select distinct sensorid from pollution_data;");
            IList<string> sensors = new List<string>();
            foreach (var s in listOfSensors)
                sensors.Add(s.GetValue<string>("sensorid"));
            return Ok(sensors);
        }

        //[HttpPost] session.Execute("INSERT INTO pollution_data (sensorid, sensordata, collecttime) VALUES ('Nisava11', 17.5, 1610231081000)");
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task Post(Pollution pollution)
        {
            _session.Execute("INSERT INTO pollution_data (sensorid, sensordata, collecttime) VALUES ('" + pollution.SensorId + "', " + pollution.SensorData + ", " + pollution.CollectTime + ")");
        }


        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            _session.Execute("DELETE FROM pollution_data WHERE sensorid='" + id + "'");
        }

        [HttpDelete(nameof(DeleteByKey))]
        public async Task DeleteByKey([FromBody] PollutionDTO poll)
        {
            _session.Execute("DELETE FROM pollution_data WHERE sensorid='" + poll.SensorId + "' AND collecttime=" + poll.CollectTime.ToUnixTimeMilliseconds() + "");
        }

    }
}
//DELETE FROM "WaterPollution".pollution_data WHERE sensorid='Nisava11'  AND collecttime>160773120000 AND collecttime<1610240718000;