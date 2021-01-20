using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterPollution.Domain
{
    public class Pollution
    {
        public string SensorId { get; set; }
        public long CollectTime { get; set; }
        public float SensorData { get; set; }

    }
}
