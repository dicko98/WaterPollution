using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using WaterPollution.Domain;

namespace WaterPollution.Infrastructure
{
    public class CassandraMappings : Cassandra.Mapping.Mappings
    {
       public CassandraMappings()
        {
            For<PollutionDTO>().TableName("pollution_data").ClusteringKey(x=>x.CollectTime)
                .Column(x => x.SensorId)
                .Column(x => x.CollectTime)
                .Column(x => x.SensorData)              
                ;
        }
    }
}
