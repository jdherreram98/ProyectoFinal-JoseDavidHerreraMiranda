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
    }
}
