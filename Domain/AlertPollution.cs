using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaterPollution.Domain
{
    public class AlertPollution
    {
        public string SensorId { get; set; }
        public DateTimeOffset CollectTime { get; set; }
        public float SensorData { get; set; }
        public string About { get; set; }
    }
}
