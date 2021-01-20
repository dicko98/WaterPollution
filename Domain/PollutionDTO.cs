using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterPollution.Domain
{
    public class PollutionDTO
    {
        public string SensorId { get; set; }
        public DateTimeOffset CollectTime { get; set; }
        public float SensorData { get; set; }
        
    }
}
