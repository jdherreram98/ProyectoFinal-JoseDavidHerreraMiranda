using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoFinal_JoseDavidHerreraMiranda.Models
{
    public class Simulation
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "product")]
        public Product Product { get; set; }

        [JsonProperty(PropertyName = "daysByWeek")]
        public int DaysByWeek { get; set; }

        [JsonProperty(PropertyName = "hoursByDay")]
        public int HoursByDay { get; set; }

        [JsonProperty(PropertyName = "daysOfSimulation")]
        public int DaysOfSimulation { get; set; }

        [JsonProperty(PropertyName = "completed")]
        public bool Completed { get; set; }

        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty(PropertyName = "lastModifiedDate")]
        public DateTime LastModifiedDate { get; set; }

        [JsonProperty(PropertyName = "active")]
        public bool Active { get; set; }
    }
}
