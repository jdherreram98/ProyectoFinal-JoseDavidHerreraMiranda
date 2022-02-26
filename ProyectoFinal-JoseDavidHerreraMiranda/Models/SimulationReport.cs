using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_JoseDavidHerreraMiranda.Models
{
    public class SimulationReport
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "simulationId")]
        public string SimulationId { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }
    }

    public class MachineProductLog
    {
        [JsonProperty(PropertyName = "simulationReportId")]
        public string SimulationReportId { get; set; }

        [JsonProperty(PropertyName = "machineId")]
        public string MachineId { get; set; }

        [JsonProperty(PropertyName = "quantityMade")]
        public int QuantityMade { get; set; }

        [JsonProperty(PropertyName = "grossProfit")]
        public double GrossProfit { get; set; }

        [JsonProperty(PropertyName = "netProfit")]
        public double NetProfit { get; set; }

        [JsonProperty(PropertyName = "state")]
        public bool State { get; set; }

        [JsonProperty(PropertyName = "logDate")]
        public DateTime LogDate { get; set; }

    }
}
