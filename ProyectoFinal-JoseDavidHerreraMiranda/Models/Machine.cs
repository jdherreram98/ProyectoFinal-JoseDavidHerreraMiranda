using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_JoseDavidHerreraMiranda.Models
{
    public class Machine
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; } 

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "costByHour")]
        public double CostByHour { get; set; }

        [JsonProperty(PropertyName = "productsByHour")]
        public int ProductsByHour { get; set; }

        [JsonProperty(PropertyName = "hoursToBeRepared")]
        public int HoursToBeRepared { get; set; }

        [JsonProperty(PropertyName = "state")]
        public bool State { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }

    }
}
